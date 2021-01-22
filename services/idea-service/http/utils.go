package http

import (
	"encoding/json"
	"errors"
	"fmt"
	"ideaservice"
	"io"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	"strings"

	"github.com/julienschmidt/httprouter"
)

var errBadRequest = errors.New("Bad request")

type httpErrorResponse struct {
	Error httpResponseError `json:"error"`
}

type httpResponseError struct {
	Code    int         `json:"code"`
	Message interface{} `json:"message,omitempty"`
}

type httpResponse struct {
	Data interface{} `json:"data,omitempty"`
}

type httpResponseData map[string]interface{}

// writes a status code and a response body with the provided message to the client.
func writeResponse(w http.ResponseWriter, code int, data interface{}) {
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(code)
	log.Printf("utils: request returned HTTP success code: %d", code)
	if data != nil {
		r := &httpResponse{Data: data}
		encoder := json.NewEncoder(w)
		encoder.Encode(r)
	}
}

// writes a status code and a response body with the provided error message(s) to the client.
func writeErrorResponse(w http.ResponseWriter, code int, messages ...string) {
	r := &httpErrorResponse{Error: httpResponseError{Code: code}}
	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(code)
	log.Printf("utils: request error: %s", strings.Join(messages, ","))
	if code == http.StatusInternalServerError {
		r.Error.Message = http.StatusText(code)
	} else {
		if messages != nil {
			if len(messages) == 1 {
				r.Error.Message = messages[0]
			} else {
				r.Error.Message = messages
			}
		}
	}
	encoder := json.NewEncoder(w)
	encoder.Encode(r)
}

// Parses the post body of a request to the specified command.
func parseCommand(body io.ReadCloser, cmd ideaservice.Command) error {
	b, err := ioutil.ReadAll(body)
	if err != nil {
		return fmt.Errorf("%w: %v", errBadRequest, err)
	}
	if err = json.Unmarshal(b, &cmd); err != nil {
		return fmt.Errorf("%w: %v", errBadRequest, err)
	}
	return nil
}

// Parses the parameter with the specified key as an integer.
func parseIntParam(ps httprouter.Params, key string) (int, error) {
	var v int
	s := ps.ByName(key)
	if s == "" {
		return v, fmt.Errorf("%w: the request parameter %s is not defined", errBadRequest, key)
	}
	v, err := strconv.Atoi(s)
	if err != nil {
		return v, fmt.Errorf("%w: the request parameter %s is not a number", errBadRequest, key)
	}
	return v, nil
}
