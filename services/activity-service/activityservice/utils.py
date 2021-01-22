import os
import sys
import json
import logging


def get_config_as_dictionary(config_file_path=None):
    """Read config file from the specified
    path and return its values as a dictionary."""

    # log an error message and exit app if file doesn't exist
    if not os.path.isfile(config_file_path):
        logging.error('Configuration file {} doesnt exist'
                      .format(config_file_path))
        sys.exit(1)

    conf = {}

    # map lines as key value pairs and add to conf
    with open(config_file_path) as fh:
        for line in fh:
            line = line.strip()
            if len(line) != 0 and line[0] != '#':
                parameter, value = line.strip().split('=', 1)
                conf[parameter] = value

    return conf


def in_container():
    """Returns a boolean indicating whether or not the current process runs
    inside a docker container."""

    path = '/proc/self/cgroup'

    return os.path.exists('/.dockerenv') or \
        os.path.isfile(path) and any('docker' in line for line in open(path))


def get_env_var(env_var_name, default_value=None):
    """Returns the value of the specified environment variable.
    If the environment variable is unset, and if default_value is not None,
    default_value will be returned. If the env var is unset, and default_value
    is None, get_env_var will raise a ValueError."""

    value = os.getenv(env_var_name)

    if not value:
        if default_value is None:
            raise ValueError(
                'Environment variable {} is unset'.format(env_var_name))

        return default_value

    return value


def get_avro_schema():
    """Reads the avro schema and parses and returns its
    contents as a JSON object."""

    with open('avro_schema.json', 'r') as f:
        data = json.load(f)
        string_data = json.dumps(data)

        return string_data


def shutdown_uwsgi():
    """Writes the character q to the master FIFO monitored by uWSGI
    and triggers graceful shutdown of the main process along with it's
    workers and mules."""

    with open('/tmp/master-fifo', 'w') as f:
        f.write('q')
        f.close()
