CREATE TABLE IF NOT EXISTS activities (
    activity_type VARCHAR(100) NOT NULL,
	created_on timestamptz NOT NULL,
	id SERIAL PRIMARY KEY,
	organization_id INT NOT NULL,
	payload JSON NOT NULL
);