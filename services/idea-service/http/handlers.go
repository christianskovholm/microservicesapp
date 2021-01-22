package http

import (
	"errors"
	"fmt"
	"ideaservice"
	"ideaservice/commands"
	"log"
	"net/http"
	"strings"

	"github.com/julienschmidt/httprouter"
)

func (s *Server) createIdea(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	c := new(commands.CreateIdea)
	if err := parseCommand(r.Body, c); err != nil {
		writeErrorResponse(w, http.StatusBadRequest, err.Error())
		return
	}
	id, err := parseIntParam(ps, "id")
	if err != nil {
		writeErrorResponse(w, http.StatusBadRequest, err.Error())
		return
	}
	c.OrganizationID = id
	var f []string
	if strings.TrimSpace(c.Name) == "" || len(c.Name) > 50 {
		f = append(f, "Name must be between 1 and 50 characters.")
	}
	if strings.TrimSpace(c.Description) == "" || len(c.Description) > 500 {
		f = append(f, "Description must be between 1 and 500 characters.")
	}
	if c.OrganizationID == 0 {
		f = append(f, "Organization id parameter must be larger than 0.")
	}
	if len(f) > 0 {
		writeErrorResponse(w, http.StatusBadRequest, f...)
		return
	}
	id, err = s.dispatcher.Dispatch(c)
	if err != nil {
		writeErrorResponse(w, http.StatusInternalServerError, err.Error())
		return
	}
	writeResponse(w, http.StatusOK, &httpResponseData{"id": id})
}

func (s *Server) getIdea(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	id, err := parseIntParam(ps, "id")
	if err != nil {
		writeErrorResponse(w, http.StatusBadRequest, err.Error())
		return
	}
	i, err := s.queries.GetIdea(id)
	if err != nil {
		writeErrorResponse(w, http.StatusInternalServerError, err.Error())
		return
	}
	if i == nil {
		writeErrorResponse(w, http.StatusNotFound, fmt.Sprintf("No idea exists with id %d.", id))
		return
	}
	writeResponse(w, http.StatusOK, i)
}

func (s *Server) getAllIdeas(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	id, err := parseIntParam(ps, "id")
	if err != nil {
		writeErrorResponse(w, http.StatusBadRequest, err.Error())
		return
	}
	i, err := s.queries.GetAllOrganizationIdeas(id)
	if err != nil {
		writeErrorResponse(w, http.StatusInternalServerError, err.Error())
		return
	}
	if i == nil {
		writeErrorResponse(w, http.StatusNotFound, fmt.Sprintf("No ideas exists with organization id %d.", id))
		return
	}
	writeResponse(w, http.StatusOK, i)
}

func (s *Server) updateIdea(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	id, err := parseIntParam(ps, "id")
	if err != nil {
		writeErrorResponse(w, http.StatusBadRequest, err.Error())
		return
	}
	c := &commands.UpdateIdea{IdeaID: id}
	if err = parseCommand(r.Body, c); err != nil {
		writeErrorResponse(w, http.StatusBadRequest, err.Error())
		return
	}
	var f []string
	if strings.TrimSpace(c.Name) == "" || len(c.Name) > 50 {
		f = append(f, "Name must be between 1 and 50 characters.")
	}
	if strings.TrimSpace(c.Description) == "" || len(c.Description) > 500 {
		f = append(f, "Description must be between 1 and 500 characters.")
	}
	if len(f) > 0 {
		writeErrorResponse(w, http.StatusBadRequest, f...)
		return
	}
	_, err = s.dispatcher.Dispatch(c)
	if err != nil {
		if errors.Is(err, ideaservice.ErrNotFound) {
			writeErrorResponse(w, http.StatusNotFound, fmt.Sprintf("No idea exists with id %d.", id))
			return
		} else {
			writeErrorResponse(w, http.StatusInternalServerError, err.Error())
			return
		}
	}
	writeResponse(w, http.StatusOK, &httpResponseData{"id": c.IdeaID})
}

func (s *Server) deleteIdea(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	id, err := parseIntParam(ps, "id")
	if err != nil {
		writeErrorResponse(w, http.StatusBadRequest, err.Error())
		return
	}
	c := &commands.DeleteIdea{IdeaID: id}
	_, err = s.dispatcher.Dispatch(c)
	if err != nil {
		if errors.Is(err, ideaservice.ErrNotFound) {
			writeErrorResponse(w, http.StatusNotFound, fmt.Sprintf("No idea exists with id %d.", id))
			return
		} else {
			writeErrorResponse(w, http.StatusInternalServerError, err.Error())
			return
		}
	}
	writeResponse(w, http.StatusNoContent, nil)
}

func (s *Server) methodNotAllowed(w http.ResponseWriter, r *http.Request) {
	writeResponse(w, http.StatusMethodNotAllowed, nil)
}

func (s *Server) notFound(w http.ResponseWriter, r *http.Request) {
	writeResponse(w, http.StatusNotFound, nil)
}

func (s *Server) panic(w http.ResponseWriter, r *http.Request, e interface{}) {
	log.Printf("http: panic during request: %v", e)
	writeResponse(w, http.StatusInternalServerError, nil)
}
