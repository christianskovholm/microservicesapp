package commands

import (
	"encoding/json"
	"ideaservice"
)

// Events represents an abstraction for saving events.
type Events struct {
}

// Save saves specified event in the specified database transaction.
func (e *Events) Save(tx ideaservice.Transaction, event *ideaservice.Event) error {
	sql := "INSERT INTO events (event_type, payload, organization_id, timestamp) VALUES ($1, $2, $3, $4)"
	j, err := json.Marshal(event.Payload)
	if err != nil {
		return err
	}
	_, err = tx.Exec(sql, event.Type, j, event.OrganizationID, event.Timestamp)
	return err
}
