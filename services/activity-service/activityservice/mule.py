import logging
from activityservice.kafka import consume
from activityservice.utils import shutdown_uwsgi
from activityservice.db import init_conn
from activityservice.config import Config, KafkaConfig

# mule.py is a separate process from the master app process.
# both are run and managed by uWSGI.

try:
    config = Config()
    kafka_config = KafkaConfig()

    init_conn(1, 1, 3, config.POSTGRES_CONN_STR,
              '{}-mule'.format(config.K8S_POD_NAME))
    consume(kafka_config)

except Exception as e:
    # if an error occurs which the error handler in the consume
    # process can not handle, shutdown the app and let the container
    # restart.
    logging.error(str(e))
    shutdown_uwsgi()
