import copy
import pytest
import werkzeug.utils
import confluent_kafka
import confluent_kafka.serialization
from activityservice.db import init_conn
from confluent_kafka.schema_registry.avro import AvroSerializer
from confluent_kafka.schema_registry import SchemaRegistryClient


@pytest.fixture
def test_config():
    """Get test config."""

    config = werkzeug.utils.import_string('activityservice.config.TestConfig')

    return config


@pytest.fixture
def kafka_config():
    """Get kafka config."""

    config = werkzeug.utils.import_string('activityservice.config.KafkaConfig')

    return config


@pytest.fixture
def conn(test_config):
    """Get a connection for use in test environments."""

    init_conn(1, 1, 3, test_config.POSTGRES_CONN_STR,
              'activity-service-test')


@pytest.fixture
def producer(kafka_config):
    """Creates and returns a kafka producer."""

    string_serializer = confluent_kafka.serialization.StringSerializer('utf_8')

    schema_registry_client = SchemaRegistryClient(
        kafka_config.SCHEMA_REGISTRY_CONFIG)

    avro_serializer = AvroSerializer(
        schema_str=kafka_config.SCHEMA_REGISTRY_AVRO_SCHEMA,
        schema_registry_client=schema_registry_client)

    producer_conf = copy.copy(kafka_config.LIBRDKAFKA_CONFIG)
    producer_conf['key.serializer'] = string_serializer
    producer_conf['value.serializer'] = avro_serializer

    producer = confluent_kafka.SerializingProducer(producer_conf)

    return producer
