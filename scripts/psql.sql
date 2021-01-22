SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE pid <> pg_backend_pid() AND datname = 'activity_service';

select pid, application_name from pg_stat_activity where datname = 'activity_service';