package pgx

import (
	"context"
	"fmt"
	"ideaservice"
	"os"

	"github.com/jackc/pgx/v4"
	"github.com/jackc/pgx/v4/pgxpool"
)

// Conn represents a connection to a postgres database using the pgx driver.
type Conn struct {
	pool *pgxpool.Pool
}

// NewPgxConn creates a new connection pool using the pgx driver.
func NewPgxConn(connStr string) *Conn {
	pool, err := pgxpool.Connect(context.Background(), connStr)
	if err != nil {
		fmt.Printf("database: unable connect to database: %v\n", err)
		os.Exit(1)
	}
	return &Conn{pool: pool}
}

func (c *Conn) QueryRow(sql string, args ...interface{}) ideaservice.Row {
	return c.pool.QueryRow(context.Background(), sql, args...)
}

func (c *Conn) Query(sql string, args ...interface{}) (ideaservice.Rows, error) {
	return c.pool.Query(context.Background(), sql, args...)
}

func (c *Conn) Exec(sql string, args ...interface{}) (ideaservice.Result, error) {
	return c.pool.Exec(context.Background(), sql, args...)
}

// Transaction represents a database transaction.
type Transaction struct {
	tx pgx.Tx
}

func (c *Conn) Begin() (ideaservice.Transaction, error) {
	tx, err := c.pool.Begin(context.Background())
	if err != nil {
		return nil, err
	}
	return &Transaction{tx: tx}, nil
}

func (t *Transaction) QueryRow(sql string, args ...interface{}) ideaservice.Row {
	return t.tx.QueryRow(context.Background(), sql, args...)
}

func (t *Transaction) Exec(sql string, args ...interface{}) (ideaservice.Result, error) {
	return t.tx.Exec(context.Background(), sql, args...)
}

func (t *Transaction) Rollback() error {
	return t.tx.Rollback(context.Background())
}

func (t *Transaction) Commit() error {
	return t.tx.Commit(context.Background())
}
