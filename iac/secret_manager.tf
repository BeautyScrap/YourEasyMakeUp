resource "google_secret_manager_secret" "bot_token" {
  secret_id = "telegram_bot_token"

  labels = {
    owner = "bot"
  }

  replication {
    auto {}
  }

  depends_on = [google_project_service.secretmanager_googleapis_com]
}

resource "google_secret_manager_secret_version" "bot_token" {
  secret      = google_secret_manager_secret.bot_token.id
  secret_data = "dummy"

  lifecycle {
    ignore_changes = [
      secret_data
    ]
  }
}

resource "google_secret_manager_secret" "postgres_con_str" {
  secret_id = "postgres_con_str"

  labels = {
    owner = "bot"
  }

  replication {
    auto {}
  }

  depends_on = [google_project_service.secretmanager_googleapis_com]
}

resource "google_secret_manager_secret_version" "postgres_con_str" {
  secret      = google_secret_manager_secret.postgres_con_str.id
  secret_data = "dummy"

  lifecycle {
    ignore_changes = [
      secret_data
    ]
  }
}

resource "google_secret_manager_secret" "atlas_priv_key" {
  secret_id = "atlas_priv_key"

  labels = {
    owner = "bot"
  }

  replication {
    auto {}
  }

  depends_on = [google_project_service.secretmanager_googleapis_com]
}

resource "google_secret_manager_secret_version" "atlas_priv_key" {
  secret      = google_secret_manager_secret.atlas_priv_key.id
  secret_data = "dummy"

  lifecycle {
    ignore_changes = [
      secret_data
    ]
  }
}

resource "google_secret_manager_secret" "atlas_org_id" {
  secret_id = "atlas_org_id"

  labels = {
    owner = "bot"
  }

  replication {
    auto {}
  }

  depends_on = [google_project_service.secretmanager_googleapis_com]
}

resource "google_secret_manager_secret_version" "atlas_org_id" {
  secret      = google_secret_manager_secret.atlas_org_id.id
  secret_data = "dummy"

  lifecycle {
    ignore_changes = [
      secret_data
    ]
  }
}

resource "google_secret_manager_secret" "atlas_pub_key" {
  secret_id = "atlas_pub_key"

  labels = {
    owner = "bot"
  }

  replication {
    auto {}
  }

  depends_on = [google_project_service.secretmanager_googleapis_com]
}

resource "google_secret_manager_secret_version" "atlas_pub_key" {
  secret      = google_secret_manager_secret.atlas_pub_key.id
  secret_data = "dummy"

  lifecycle {
    ignore_changes = [
      secret_data
    ]
  }
}