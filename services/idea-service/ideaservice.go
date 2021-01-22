package ideaservice

import (
	"errors"
	"time"
)

// ErrNotFound represents an error where no results are found.
var ErrNotFound = errors.New("not found")

// MsgNoRows default error message returned by the database driver.
var MsgNoRows = "no rows in result set"

// Idea represents an idea.
type Idea struct {
	ID             int       `json:"id"`
	OrganizationID int       `json:"organizationId"`
	Name           string    `json:"name"`
	Description    string    `json:"description"`
	CreatedOn      time.Time `json:"createdOn"`
	LastUpdated    time.Time `json:"lastUpdated"`
}

// Rows represents a set of rows in a database table.
type Rows interface {
	Close()
	Err() error
	Next() bool
	Scan(dest ...interface{}) error
}

// Row represents a row in a database table.
type Row interface {
	Scan(dest ...interface{}) error
}

// Result represents a result from a database query.
type Result interface {
	RowsAffected() int64
}

// Transaction represents a database transaction.
type Transaction interface {
	Rollback() error
	Commit() error
	QueryRow(sql string, args ...interface{}) Row
	Exec(sql string, args ...interface{}) (Result, error)
}

// Command represents a command in CQRS terminology.
type Command interface {
	Handle(tx Transaction, timestamp time.Time) (*Event, error)
}

// Event is a side effect of a transaction describing the outcome of a transaction.
type Event struct {
	IdeaID         int
	OrganizationID int
	Payload        map[string]interface{}
	Timestamp      time.Time
	Type           string
}

// Events is an abstraction for a mechanism used to save events.
type Events interface {
	Save(tx Transaction, e *Event) error
}
