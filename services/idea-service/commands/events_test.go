package commands

import (
	"ideaservice"
	"ideaservice/testutils"
	"testing"
	"time"

	"github.com/stretchr/testify/assert"
)

func TestSave(t *testing.T) {
	var id int
	events := new(Events)
	timestamp := time.Now().UTC()
	tx, _ := testutils.GetTestConn().Begin()
	sql := "INSERT INTO ideas (created_on, description, last_updated, name, organization_id) VALUES ($1, $2, $3, $4, $5) RETURNING id"
	err := tx.QueryRow(sql, timestamp, "test", timestamp, "test", 1).Scan(&id)
	if err != nil {
		t.Fatal(err)
	}
	e := &ideaservice.Event{
		IdeaID:         id,
		OrganizationID: 1,
		Payload:        map[string]interface{}{"id": id, "name": "test"},
		Timestamp:      timestamp,
		Type:           ideaCreated,
	}
	err = events.Save(tx, e)
	assert.Nil(t, err)
}
