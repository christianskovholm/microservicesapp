package main

import (
	"fmt"
	"ideaservice/commands"
	"ideaservice/http"
	"ideaservice/pgx"
	"ideaservice/queries"
	"os"
)

func main() {
	connStr := "POSTGRES_CONN_STR"
	cs, ok := os.LookupEnv(connStr)
	if !ok {
		fmt.Printf("main: environment variable %s is unset", connStr)
		os.Exit(1)
	}
	c := pgx.NewPgxConn(cs)
	d := commands.NewDispatcher(c, &commands.Events{})
	q := queries.NewQueries(c)
	s := http.NewServer(d, q)
	s.Serve(8080)
}
