package testutils

import (
	"ideaservice"
	"ideaservice/pgx"
	"os"
)

var conn *pgx.Conn

// GetTestConn returns a database connection for use in test environments.
func GetTestConn() *pgx.Conn {
	if conn == nil {
		s, ok := os.LookupEnv("POSTGRES_CONN_STR")
		if !ok {
			s = "postgresql://postgres:password@localhost:5432/idea_service?sslmode=disable"
		}
		conn = pgx.NewPgxConn(s)
	}
	return conn
}

// CreateIdeas is a helper function for seeding a database before tests.
func CreateIdeas(ideas ...*ideaservice.Idea) error {
	for _, i := range ideas {
		sql := "INSERT INTO ideas (created_on, description, last_updated, name, organization_id) VALUES ($1, $2, $3, $4, $5) RETURNING id"
		if err := GetTestConn().QueryRow(sql, i.CreatedOn, i.Description, i.LastUpdated, i.Name, i.OrganizationID).Scan(&i.ID); err != nil {
			return err
		}
	}
	return nil
}
