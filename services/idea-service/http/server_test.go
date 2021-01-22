package http

import (
	"bytes"
	"fmt"
	"ideaservice"
	"ideaservice/commands"
	"ideaservice/queries"
	"ideaservice/testutils"
	"net/http"
	"net/http/httptest"
	"os"
	"testing"
	"time"

	"github.com/stretchr/testify/assert"
)

var ts *httptest.Server

func TestMain(m *testing.M) {
	d := commands.NewDispatcher(testutils.GetTestConn(), new(commands.Events))
	q := queries.NewQueries(testutils.GetTestConn())
	s := NewServer(d, q)
	ts = httptest.NewServer(s.router)
	code := m.Run()
	os.Exit(code)
}

func TestCreateIdea(t *testing.T) {
	url := fmt.Sprintf("%s/organizations/1/ideas", ts.URL)
	body := bytes.NewReader([]byte(`{"name": "test", "description": "test", "organizationId": 1}`))
	res, err := ts.Client().Post(url, "application/json", body)
	assert.Equal(t, http.StatusOK, res.StatusCode)
	assert.Nil(t, err)
}

func TestGetIdea(t *testing.T) {
	i := &ideaservice.Idea{}
	if err := testutils.CreateIdeas(i); err != nil {
		t.Fatal(err)
	}
	url := fmt.Sprintf("%s/ideas/%d", ts.URL, i.ID)
	res, err := ts.Client().Get(url)
	assert.Equal(t, http.StatusOK, res.StatusCode)
	assert.Nil(t, err)
}

func TestGetAllIdeas(t *testing.T) {
	i := &ideaservice.Idea{OrganizationID: int(time.Now().Unix())}
	if err := testutils.CreateIdeas(i); err != nil {
		t.Fatal(err)
	}
	url := fmt.Sprintf("%s/organizations/%d/ideas", ts.URL, i.OrganizationID)
	res, err := ts.Client().Get(url)
	assert.Equal(t, http.StatusOK, res.StatusCode)
	assert.Nil(t, err)
}

func TestUpdateIdea(t *testing.T) {
	i := &ideaservice.Idea{}
	if err := testutils.CreateIdeas(i); err != nil {
		t.Fatal(err)
	}
	body := bytes.NewReader([]byte(`{"name": "test", "description": "test"}`))
	url := fmt.Sprintf("%s/ideas/%d", ts.URL, i.ID)
	req, err := http.NewRequest("PUT", url, body)
	if err != nil {
		t.Fatal(err)
	}
	res, err := ts.Client().Do(req)
	assert.Equal(t, http.StatusOK, res.StatusCode)
	assert.Nil(t, err)
}

func TestDeleteIdea(t *testing.T) {
	i := &ideaservice.Idea{}
	if err := testutils.CreateIdeas(i); err != nil {
		t.Fatal(err)
	}
	url := fmt.Sprintf("%s/ideas/%d", ts.URL, i.ID)
	req, err := http.NewRequest("DELETE", url, nil)
	if err != nil {
		t.Fatal(err)
	}
	res, err := ts.Client().Do(req)
	assert.Equal(t, http.StatusNoContent, res.StatusCode)
	assert.Nil(t, err)
}
