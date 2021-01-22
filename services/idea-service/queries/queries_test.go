package queries

import (
	"ideaservice"
	"ideaservice/testutils"
	"math/rand"
	"os"
	"testing"
	"time"

	"github.com/stretchr/testify/assert"
)

var q *Queries

func TestMain(m *testing.M) {
	q = &Queries{DbConn: testutils.GetTestConn()}
	code := m.Run()
	os.Exit(code)
}

func TestGetIdea(t *testing.T) {
	timestamp := time.Now()
	testCases := []struct {
		desc         string
		expectedIdea *ideaservice.Idea
	}{
		{
			"idea should equal values of tc.idea",
			&ideaservice.Idea{CreatedOn: timestamp, Description: "test", LastUpdated: timestamp, Name: "test", OrganizationID: 1},
		},
		{
			"idea should equal nil",
			nil,
		},
	}
	for _, tc := range testCases {
		t.Run(tc.desc, func(t *testing.T) {
			rand.Seed(time.Now().UnixNano())
			id := 1000 + rand.Intn(10000-1000)
			if tc.expectedIdea != nil {
				if err := testutils.CreateIdeas(tc.expectedIdea); err != nil {
					t.Fatal(err)
				}
				id = tc.expectedIdea.ID
			}
			idea, err := q.GetIdea(id)
			assert.ObjectsAreEqualValues(tc.expectedIdea, idea)
			assert.Equal(t, nil, err)
		})
	}
}

func TestGetAllOrganizationIdeas(t *testing.T) {
	rand.Seed(time.Now().UnixNano())
	oid := 1000 + rand.Intn(10000-1000)
	time := time.Now()
	testCases := []struct {
		desc  string
		ideas []*ideaservice.Idea
	}{
		{
			"ideas should equal values of tc.ideas",
			[]*ideaservice.Idea{
				{CreatedOn: time, Description: "test", LastUpdated: time, Name: "test", OrganizationID: oid},
				{CreatedOn: time, Description: "test", LastUpdated: time, Name: "test", OrganizationID: oid},
			},
		},
		{
			"ideas should equal nil",
			nil,
		},
	}
	for _, tc := range testCases {
		t.Run(tc.desc, func(t *testing.T) {
			if tc.ideas != nil {
				if err := testutils.CreateIdeas(tc.ideas...); err != nil {
					t.Fatal(err)
				}
			}
			ideas, err := q.GetAllOrganizationIdeas(oid)
			assert.ObjectsAreEqualValues(tc.ideas, ideas)
			assert.Equal(t, nil, err)
			oid++
		})
	}
}
