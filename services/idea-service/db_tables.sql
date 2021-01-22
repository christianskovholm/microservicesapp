CREATE TABLE IF NOT EXISTS ideas (
	created_on timestamptz NOT NULL,
	description VARCHAR(500) NOT NULL,
	id SERIAL PRIMARY KEY,
	last_updated timestamptz NOT NULL,
	name VARCHAR (50),
	organization_id INT
);

CREATE TABLE IF NOT EXISTS events (
	event_type VARCHAR(100) NOT NULL,
	id SERIAL PRIMARY KEY,
	payload JSON NOT NULL,
	organization_id INT NOT NULL,
	timestamp timestamptz NOT NULL
);