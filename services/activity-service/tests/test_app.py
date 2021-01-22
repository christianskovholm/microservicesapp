import uuid
import json
import time
import pytest
import datetime
import requests
import subprocess


def test_create_app(kafka_config, producer):
    """Produce messages to the kafka topics that the service
    subscribes to, and assert that the messages (activities) can be
    retrieved from the get activities endpoint."""

    organization_id = int(str(uuid.uuid4().int)[:8])
    msg = dict(event_type='test_event_type',
               timestamp=datetime.datetime.now(),
               organization_id=organization_id, payload=json.dumps({}), id=1)

    for x in kafka_config.KAFKA_SUBSCRIBE_TOPICS:
        producer.produce(x, key=uuid.uuid4().hex, value=msg)

    producer.flush()

    uwsgi_process = subprocess.Popen(['pipenv', 'run', 'uwsgi', 'app.ini'],
                                     stderr=subprocess.PIPE,
                                     universal_newlines=True)

    # give the service some time to startup and process the messages
    time.sleep(6)

    if uwsgi_process.poll() is not None:
        pytest.fail(uwsgi_process.stderr.read())

    response = requests.get(
        'http://localhost:5000/organizations/{}/activities'
        .format(organization_id))

    uwsgi_process.terminate()

    assert response.status_code == 200
