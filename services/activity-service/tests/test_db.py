import uuid
import json
import datetime
from activityservice.db import (
    Activity, create_activity,
    get_organization_activities
)


def test_create_activity(conn):
    """Assert that an activity can be created."""

    activity = Activity('idea_created', datetime.datetime.now(),
                        1, json.dumps({}))
    id = create_activity(activity)

    assert id > 0


def test_get_organization_activities(conn):
    """Assert that activities can be retrieved by organization id."""

    now = datetime.datetime.now().replace(microsecond=0)
    organization_id = int(str(uuid.uuid4().int)[:8])
    activity_1 = Activity('organization_created', now,
                          organization_id, json.dumps({}))
    activity_2 = Activity('idea_created', now,
                          organization_id, json.dumps({}))

    create_activity(activity_1)
    create_activity(activity_2)

    activities = get_organization_activities(organization_id)

    assert len(activities) == 2
