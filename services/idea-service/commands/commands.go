package commands

import (
	"ideaservice"
	"log"
	"time"
)

var (
	ideaCreated = "idea_created"
	ideaUpdated = "idea_updated"
)

// CreateIdea represents a command for creating a new idea.
type CreateIdea struct {
	Name           string
	Description    string
	OrganizationID int
}

// Handle creates the idea, and returns an event representing the creation.
func (c *CreateIdea) Handle(tx ideaservice.Transaction, timestamp time.Time) (*ideaservice.Event, error) {
	var id int
	sql := "INSERT INTO ideas (created_on, description, last_updated, name, organization_id) VALUES ($1, $2, $3, $4, $5) RETURNING id"
	err := tx.QueryRow(sql, timestamp, c.Description, timestamp, c.Name, c.OrganizationID).Scan(&id)
	if err != nil {
		return nil, err
	}
	log.Printf("commands: successfully created idea '%s' with id: %d", c.Name, id)
	return &ideaservice.Event{
		IdeaID:         id,
		OrganizationID: c.OrganizationID,
		Payload:        map[string]interface{}{"id": id, "name": c.Name},
		Timestamp:      timestamp,
		Type:           ideaCreated}, nil
}

// UpdateIdea represents a command for updating an idea.
type UpdateIdea struct {
	Name        string
	Description string
	IdeaID      int
}

// Handle updates the idea, and returns an event representing the update.
func (c *UpdateIdea) Handle(tx ideaservice.Transaction, timestamp time.Time) (*ideaservice.Event, error) {
	var oid int
	sql := "UPDATE ideas SET description = $1, last_updated = $2, name = $3 WHERE id = $4 RETURNING organization_id"
	if err := tx.QueryRow(sql, c.Description, timestamp, c.Name, c.IdeaID).Scan(&oid); err != nil {
		if err.Error() == ideaservice.MsgNoRows {
			return nil, ideaservice.ErrNotFound
		}
		return nil, err
	}
	log.Printf("commands: successfully updated idea '%s' with id: %d", c.Name, c.IdeaID)
	return &ideaservice.Event{
		IdeaID:         c.IdeaID,
		OrganizationID: oid,
		Payload:        map[string]interface{}{"id": c.IdeaID, "name": c.Name},
		Timestamp:      timestamp,
		Type:           ideaUpdated}, nil
}

// DeleteIdea represents a command for deleting an idea.
type DeleteIdea struct {
	IdeaID int
}

// Handle deletes the idea.
func (c *DeleteIdea) Handle(tx ideaservice.Transaction, timestamp time.Time) (*ideaservice.Event, error) {
	sql := "DELETE FROM ideas WHERE id = $1"
	res, err := tx.Exec(sql, c.IdeaID)
	if err != nil {
		return nil, err
	}
	if res.RowsAffected() == 0 {
		return nil, ideaservice.ErrNotFound
	}
	log.Printf("commands: successfully deleted idea with id: %d", c.IdeaID)
	return nil, nil
}
