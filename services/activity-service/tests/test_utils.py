import activityservice.utils as utils


def test_get_env_var_returns_env_var_value(monkeypatch):
    """Assert that an env var value can be retrieved."""

    name = 'TEST_ENV_VAR'
    value = 'VALUE'

    monkeypatch.setenv(name, value)

    assert utils.get_env_var(name) == value


def test_get_env_var_returns_default_value():
    """Assert that a placeholder for an env var value can be retrieved."""

    default_value = 'DEFAULT_VALUE'
    value = utils.get_env_var('TEST_ENV_VAR', default_value)

    assert value == default_value
