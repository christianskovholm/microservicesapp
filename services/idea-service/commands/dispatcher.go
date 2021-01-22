package commands

import (
	"ideaservice"
	"time"
)

// DbConn represents a database connection.
type DbConn interface {
	Begin() (ideaservice.Transaction, error)
}

// Dispatcher represents an abstraction for dispatching commands.
type Dispatcher struct {
	dbConn DbConn
	events ideaservice.Events
}

// NewDispatcher creates a new Dispatcher instance.
func NewDispatcher(dbConn DbConn, events ideaservice.Events) *Dispatcher {
	return &Dispatcher{dbConn: dbConn, events: events}
}

// Dispatch executes the specified command in a database transaction.
func (d *Dispatcher) Dispatch(c ideaservice.Command) (int, error) {
	var id int
	tx, err := d.dbConn.Begin()
	if err != nil {
		return 0, err
	}
	defer func() {
		if err != nil {
			tx.Rollback()
		}
	}()
	now := time.Now().UTC().Round(time.Second)
	e, err := c.Handle(tx, now)
	if err != nil {
		return 0, err
	}
	if e != nil {
		err = d.events.Save(tx, e)
		if err != nil {
			return 0, err
		}
		id = e.IdeaID
	} else {
		id = 0
	}
	err = tx.Commit()
	if err != nil {
		return 0, err
	}
	return id, err
}
