import logging
from .config import KafkaConfig
from .db import Activity, create_activity
from confluent_kafka import DeserializingConsumer
from confluent_kafka.serialization import StringDeserializer
from confluent_kafka.schema_registry import SchemaRegistryClient
from confluent_kafka.schema_registry.avro import AvroDeserializer


def message_to_activity(obj, ctx):
    """Converts object literal(dict) to an Activity instance."""

    if obj is None:
        return obj

    return Activity(activity_type=obj['event_type'],
                    created_on=obj['timestamp'],
                    organization_id=obj['organization_id'],
                    payload=obj['payload'])


def err_cb(error):
    """Default callback for kafka consumer errors."""

    logging.error(error)


def consume(config: KafkaConfig, msg_cb=create_activity, err_cb=err_cb):
    """Consumes messages from the kafka topics specified in the
    configuration. Calls the msg_cb func on message with the message
    as the single parameter."""

    string_deserializer = StringDeserializer('utf_8')

    schema_registry_client = SchemaRegistryClient(
        config.SCHEMA_REGISTRY_CONFIG)

    avro_deserializer = AvroDeserializer(
        schema_str=config.SCHEMA_REGISTRY_AVRO_SCHEMA,
        schema_registry_client=schema_registry_client,
        from_dict=message_to_activity)

    consumer_conf = config.KAFKA_CONSUMER_CONFIG

    # add consumer specific values to configuration
    consumer_conf['error_cb'] = err_cb
    consumer_conf['value.deserializer'] = avro_deserializer
    consumer_conf['key.deserializer'] = string_deserializer

    # create consumer and subscribe to topics
    consumer = DeserializingConsumer(consumer_conf)
    consumer.subscribe(config.KAFKA_SUBSCRIBE_TOPICS)

    logging.info('Polling for messages.')

    while config.POLL:
        msg = consumer.poll(0.5)

        if msg is None:
            continue

        if msg.error():
            logging.error('Consumer error: {}'.format(msg.error()))
            continue

        logging.info('Received message with key {} from topic {}'
                     .format(msg.key(), msg.topic()))
        msg_cb(msg.value())
