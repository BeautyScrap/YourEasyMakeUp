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