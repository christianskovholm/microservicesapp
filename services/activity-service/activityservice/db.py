import time
import logging
from threading import Semaphore
from psycopg2.extras import DictCursor
from psycopg2.pool import ThreadedConnectionPool


conn = None


class Activity:
    """Represents an event that has happened. The main information
    about the activity is stored as a serialized JSON object in the payload
    property."""

    def __init__(self, activity_type, created_on,
                 organization_id, payload):
        self.activity_type = activity_type
        self.created_on = created_on
        self.organization_id = organization_id
        self.payload = payload
        self.id = 0


class Connection:
    """Represents a database connection. By making an instance of this
    class globally available, any part of the application can use the
    connection for making queries against the database."""

    def __init__(self, min_conn, max_conn, max_retries, conn_str, app_name):
        self._poll = ConnectionPool(min_conn, max_conn, conn_str, **{
            'application_name': app_name
        })
        self._max_retries = max_retries

    def execute(self, sql, params=None, retry_counter=0):
        """Executes a query from the specified sql statement with the optional
        params. In case of query failure, the query will retry up to the amount
        of times specified by the self._max_retries property."""

        conn = self._poll.getconn()
        cur = conn.cursor(cursor_factory=DictCursor)
        success = False

        try:
            cur.execute(sql, params)
            conn.commit()

            success = True
        except Exception as e:
            if retry_counter >= self._max_retries:
                raise e
            else:
                retry_counter += 1

                logging.error(
                    ('Error during query execution: {0}. '
                     'Retry attempt {1} initiated.'
                     .format(str(e).strip(), retry_counter)))

                time.sleep(0.25)

        self._poll.putconn(conn)

        if success is False:
            return self.execute(sql, params, retry_counter)

        return cur


class ConnectionPool(ThreadedConnectionPool):
    """An implementation of the psycopg2 ThreadedConnectionPool
    that implements a semaphore to prevent race conditions."""

    def __init__(self, minconn, maxconn, *args, **kwargs):
        self._semaphore = Semaphore(maxconn)
        super().__init__(minconn, maxconn, *args, **kwargs)

    def getconn(self, *args, **kwargs):
        self._semaphore.acquire()
        conn = super().getconn(*args, **kwargs)

        return conn

    def putconn(self, *args, **kwargs):
        super().putconn(*args, **kwargs)
        self._semaphore.release()


def init_conn(min_conn, max_conn, max_retries, conn_str: str, app_name: str):
    """Initializes a thread safe connection in the global conn var."""

    global conn

    conn = Connection(min_conn, max_conn, max_retries, conn_str, app_name)


def create_activity(activity: Activity):
    """Create the specified activity in the database."""

    logging.info('Creating activity {}'.format(activity.activity_type))

    sql = ('INSERT INTO activities (activity_type, created_on, organization_id'
           ', payload) VALUES (%s, %s, %s, %s) RETURNING id')

    params = (activity.activity_type, activity.created_on,
              activity.organization_id, activity.payload)

    global conn

    with conn.execute(sql, params) as cur:
        id = cur.fetchone()[0]

    logging.info('Created activity {} with id {}'
                 .format(activity.activity_type, id))

    return id


def get_organization_activities(organization_id=0):
    """Return all activities with the specified organization_id."""

    global conn

    sql = ('SELECT id, activity_type, created_on, organization_id, payload '
           'FROM activities WHERE organization_id = %s '
           'ORDER BY created_on DESC')

    activities = []

    with conn.execute(sql, (organization_id,)) as cur:
        results = cur.fetchall()
        [activities.append(dict(row)) for row in results]

    return activities
