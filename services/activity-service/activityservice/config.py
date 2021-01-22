from copy import copy
from .utils import (
    get_config_as_dictionary,
    get_env_var,
    get_avro_schema,
    in_container
)


class KafkaConfig:
    """Kafka configuration."""

    LIBRDKAFKA_CONFIG_FILE_PATH = get_env_var(
        'LIBRDKAFKA_CONFIG_FILE_PATH',
        '../../confluent/kafka/librdkafka.config')

    LIBRDKAFKA_CONFIG = get_config_as_dictionary(
        LIBRDKAFKA_CONFIG_FILE_PATH)

    SCHEMA_REGISTRY_CONFIG_FILE_PATH = get_env_var(
        'SCHEMA_REGISTRY_CONFIG_FILE_PATH',
        '../../confluent/schema-registry/schema-registry.config')

    SCHEMA_REGISTRY_CONFIG = get_config_as_dictionary(
        SCHEMA_REGISTRY_CONFIG_FILE_PATH)

    if in_container() is False:
        LIBRDKAFKA_CONFIG['bootstrap.servers'] = 'localhost'
        SCHEMA_REGISTRY_CONFIG['url'] = 'http://127.0.0.1:8081'

    SCHEMA_REGISTRY_AVRO_SCHEMA = get_avro_schema()
    KAFKA_SUBSCRIBE_TOPICS = ['idea-service-events',
                              'organization-service-events']

    KAFKA_CONSUMER_CONFIG = copy(LIBRDKAFKA_CONFIG)
    KAFKA_CONSUMER_CONFIG['auto.offset.reset'] = 'earliest'
    KAFKA_CONSUMER_CONFIG['group.id'] = 'activity-service-group'

    POLL = True


class Config:
    """Main configuration."""

    POSTGRES_CONN_STR = get_env_var(
        'POSTGRES_CONN_STR',
        'postgresql://postgres:password@localhost:5432/activity_service')

    K8S_POD_NAME = get_env_var('K8S_POD_NAME', 'activity-service')


class TestConfig(Config):
    """Test configuration."""

    TESTING = True
