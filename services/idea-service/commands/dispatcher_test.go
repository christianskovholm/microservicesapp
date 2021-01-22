package commands

import (
	"ideaservice"
	"ideaservice/testutils"
	"testing"
	"time"

	"github.com/stretchr/testify/assert"
)

type CommandMock struct {
	handleReturnEvent *ideaservice.Event
	handleReturnErr   error
}

func (cm *CommandMock) Handle(ideaservice.Transaction, time.Time) (*ideaservice.Event, error) {
	return cm.handleReturnEvent, cm.handleReturnErr
}

type EventsMock struct {
	saveReturnErr error
}

func (em *EventsMock) Save(ideaservice.Transaction, *ideaservice.Event) error {
	return em.saveReturnErr
}

func TestDispatch(t *testing.T) {
	testCases := []struct {
		command     CommandMock
		event       EventsMock
		expectedErr error
	}{
		{
			CommandMock{
				handleReturnEvent: &ideaservice.Event{IdeaID: 1},
				handleReturnErr:   nil,
			},
			EventsMock{saveReturnErr: nil},
			nil,
		},
	}
	for _, tc := range testCases {
		d := NewDispatcher(testutils.GetTestConn(), &tc.event)
		id, err := d.Dispatch(&tc.command)
		assert.Equal(t, tc.expectedErr, err)
		assert.Equal(t, tc.command.handleReturnEvent.IdeaID, id)
	}
}
