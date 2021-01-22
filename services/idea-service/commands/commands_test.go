package commands

import (
	"ideaservice"
	"ideaservice/testutils"
	"testing"
	"time"

	"github.com/stretchr/testify/assert"
)

func TestCreateIdea(t *testing.T) {
	timestamp := time.Now().UTC()
	c := &CreateIdea{Description: "test", Name: "test", OrganizationID: 1}
	tx, _ := testutils.GetTestConn().Begin()
	e, err := c.Handle(tx, timestamp)
	expectedEvent := &ideaservice.Event{
		IdeaID:         e.IdeaID,
		OrganizationID: c.OrganizationID,
		Payload:        map[string]interface{}{"id": e.IdeaID, "name": c.Name},
		Timestamp:      timestamp,
		Type:           ideaCreated,
	}
	assert.Nil(t, err)
	assert.EqualValues(t, expectedEvent, e)
}

func TestUpdateIdea(t *testing.T) {
	idea := &ideaservice.Idea{Name: "test", Description: "test"}
	err := testutils.CreateIdeas(idea)
	if err != nil {
		t.Fatal(err)
	}
	c := &UpdateIdea{Name: "test updated", Description: "test updated", IdeaID: idea.ID}
	tx, _ := testutils.GetTestConn().Begin()
	timestamp := time.Now().UTC()
	e, err := c.Handle(tx, timestamp)
	expectedEvent := &ideaservice.Event{
		IdeaID:         idea.ID,
		OrganizationID: idea.OrganizationID,
		Payload:        map[string]interface{}{"id": idea.ID, "name": c.Name},
		Timestamp:      timestamp,
		Type:           ideaUpdated,
	}
	assert.Equal(t, nil, err)
	assert.EqualValues(t, expectedEvent, e)
}

func TestDeleteIdea(t *testing.T) {
	timestamp := time.Now().UTC()
	testCases := []struct {
		idea        *ideaservice.Idea
		expectedErr error
	}{
		{
			&ideaservice.Idea{Name: "test"},
			nil,
		},
		{
			&ideaservice.Idea{},
			ideaservice.ErrNotFound,
		},
	}
	for _, tc := range testCases {
		if tc.idea.Name != "" {
			if err := testutils.CreateIdeas(tc.idea); err != nil {
				t.Fatal(err)
			}
		}
		c := &DeleteIdea{IdeaID: tc.idea.ID}
		tx, _ := testutils.GetTestConn().Begin()
		_, err := c.Handle(tx, timestamp)
		assert.Equal(t, tc.expectedErr, err)
	}
}
