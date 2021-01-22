import logging
from flask import Flask
from .config import Config


def create_logging_config():
    """Create and return logging configuration."""

    return {
        'version': 1,
        'formatters': {'default': {
            'format': ('[%(asctime)s] %(levelname)s '
                       'in %(module)s:%(funcName)s: %(message)s')
        }},
        'handlers': {'wsgi': {
            'class': 'logging.StreamHandler',
            'stream': 'ext://flask.logging.wsgi_errors_stream',
            'formatter': 'default'
        }},
        'root': {
            'level': 'INFO',
            'handlers': ['wsgi']
        }
    }


def register_error_handlers(app: Flask):
    """Register error handlers."""

    def not_found(e):
        logging.info(str(e))
        return '', 404

    def error_handler(e):
        logging.error(str(e))
        return '', 500

    app.register_error_handler(404, not_found)
    app.register_error_handler(500, error_handler)


def create_app(config: Config = Config):
    """Creates and configures a Flask application."""

    # configure logging
    from logging.config import dictConfig

    dictConfig(create_logging_config())

    # create app object
    app = Flask(__name__)

    # init conn pool
    from .db import init_conn

    init_conn(1, 2, 3, config.POSTGRES_CONN_STR,
              '{}-app'.format(config.K8S_POD_NAME))

    # register routes and error handlers
    from .blueprint import bp

    app.register_blueprint(bp)
    register_error_handlers(app)

    return app
