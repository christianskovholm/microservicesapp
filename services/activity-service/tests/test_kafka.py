import json
import uuid
import datetime
import pytest # NOQA
from activityservice.db import Activity
from activityservice.kafka import consume
from confluent_kafka.admin import AdminClient
from activityservice.utils import get_config_as_dictionary


def test_get_librdkafka_config(kafka_config):
    """Ensure that a LIBRDKAFKA config file can be read from the file system,
    and that it's contents can be parsed to a dict."""

    kafka_config = get_config_as_dictionary(
        kafka_config.LIBRDKAFKA_CONFIG_FILE_PATH)

    assert kafka_config['bootstrap.servers'] != ""


def test_consume(kafka_config, producer):
    """Ensure that consume correctly receives messages produced to the topic
    which the consumer subscribes to."""

    msg = dict(event_type='test_event_type',
               timestamp=datetime.datetime.now(
                   tz=datetime.timezone.utc).replace(microsecond=0),
               organization_id=1, payload=json.dumps({}), id=1)

    kafka_config.KAFKA_SUBSCRIBE_TOPICS = [uuid.uuid4().hex]
    admin = AdminClient(kafka_config.LIBRDKAFKA_CONFIG)

    producer.produce(
        kafka_config.KAFKA_SUBSCRIBE_TOPICS[0],
        key=uuid.uuid4().hex, value=msg)
    producer.flush()

    def msg_callback(activity: Activity):
        assert activity.activity_type == msg['event_type'] and \
            activity.created_on == msg['timestamp'] and \
            activity.organization_id == msg['organization_id'] and \
            activity.payload == msg['payload']

        kafka_config.POLL = False

    consume(config=kafka_config, msg_cb=msg_callback)
    admin.delete_topics(kafka_config.KAFKA_SUBSCRIBE_TOPICS)
