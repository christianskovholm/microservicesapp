package http

import (
	"fmt"
	"ideaservice"
	"log"
	"net/http"

	"github.com/julienschmidt/httprouter"
)

// Represents a request handler.
type handler func(*http.Request, httprouter.Params) (*httpResponse, error)

// Represents a response body returned to a client in
// case an error occurs during request execution.
type errorResponse struct {
	Status  int    `json:"status"`
	Message string `json:"message"`
}

type serverDispatcher interface {
	Dispatch(ideaservice.Command) (int, error)
}

type serverQueries interface {
	GetIdea(id int) (*ideaservice.Idea, error)
	GetAllOrganizationIdeas(organizationID int) ([]*ideaservice.Idea, error)
}

// Server is an abstraction over the underlying HTTP router.
// Provides the routes with implementations of their dependencies.
type Server struct {
	dispatcher serverDispatcher
	queries    serverQueries
	router     *httprouter.Router
}

// NewServer creates a new Server struct and configures it
// with request handlers.
func NewServer(d serverDispatcher, q serverQueries) *Server {
	s := &Server{
		dispatcher: d,
		queries:    q,
		router:     httprouter.New(),
	}
	s.router.POST("/organizations/:id/ideas", s.createIdea)
	s.router.GET("/ideas/:id", s.getIdea)
	s.router.GET("/organizations/:id/ideas", s.getAllIdeas)
	s.router.PUT("/ideas/:id", s.updateIdea)
	s.router.DELETE("/ideas/:id", s.deleteIdea)
	s.router.MethodNotAllowed = http.HandlerFunc(s.methodNotAllowed)
	s.router.NotFound = http.HandlerFunc(s.notFound)
	s.router.HandleMethodNotAllowed = true
	s.router.PanicHandler = s.panic
	return s
}

// Serve serves the application and listens for incoming requests.
func (s *Server) Serve(port int) {
	log.Printf("http: listening on port %d", port)
	log.Fatal(http.ListenAndServe(fmt.Sprintf(":%d", port), s.router))
}
