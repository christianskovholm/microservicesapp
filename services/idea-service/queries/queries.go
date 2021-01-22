package queries

import (
	"ideaservice"
)

// DbConn represents a database connection.
type DbConn interface {
	QueryRow(sql string, args ...interface{}) ideaservice.Row
	Query(sql string, args ...interface{}) (ideaservice.Rows, error)
}

// Queries is the read/query implementation of the CQRS pattern.
// Contains a set of predefined queries that can be called to retrieve ideas.
type Queries struct {
	DbConn DbConn
}

// NewQueries returns a new instance of Queries.
func NewQueries(dbConn DbConn) *Queries {
	return &Queries{DbConn: dbConn}
}

// GetIdea returns the specified idea.
func (q *Queries) GetIdea(id int) (*ideaservice.Idea, error) {
	i := new(ideaservice.Idea)
	sql := "SELECT created_on, description, id, last_updated, name, organization_id FROM ideas WHERE id = $1"
	if err := q.DbConn.QueryRow(sql, id).Scan(&i.CreatedOn, &i.Description, &i.ID, &i.LastUpdated, &i.Name, &i.OrganizationID); err != nil {
		if err.Error() == ideaservice.MsgNoRows {
			return nil, nil
		}
		return nil, err
	}
	return i, nil
}

// GetAllOrganizationIdeas returns all ideas belonging to an organization.
func (q *Queries) GetAllOrganizationIdeas(organizationID int) ([]*ideaservice.Idea, error) {
	sql := "SELECT created_on, description, id, last_updated, name, organization_id FROM ideas WHERE organization_id = $1 ORDER BY created_on DESC"
	r, err := q.DbConn.Query(sql, organizationID)
	if err != nil {
		return nil, err
	}
	defer r.Close()
	var ideas []*ideaservice.Idea
	for r.Next() {
		i := new(ideaservice.Idea)
		if err := r.Scan(&i.CreatedOn, &i.Description, &i.ID, &i.LastUpdated, &i.Name, &i.OrganizationID); err != nil {
			return nil, err
		}
		ideas = append(ideas, i)
	}
	if err = r.Err(); err != nil {
		return nil, err
	}
	return ideas, nil
}
