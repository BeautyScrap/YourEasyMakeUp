resource "google_secret_manager_secret" "bot_token" {
  secret_id = "telegram_bot_token"

  labels = {
    owner = "bot"
  }

  replication {
    automatic = true
  }
}