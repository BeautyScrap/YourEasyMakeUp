resource "google_cloud_run_service" "bot" {
  name     = var.bot_name
  location = var.region
  project  = var.project_id

  autogenerate_revision_name = true

  lifecycle {
    ignore_changes = [template[0].spec[0].containers[0].image]
  }

  template {
    spec {
      containers {
        image = local.bot_image_url

        ports {
          container_port = var.port
        }

        resources {
          limits = {
            cpu    = "2000m"
            memory = "1Gi"
          }
        }

        env {
          name  = "TELEGRAM_BOT_TOKEN"
          value = google_secret_manager_secret_version.bot_token.secret_data
        }

        env {
          name  = "ASPNETCORE_ENVIRONMENT"
          value = "Production"
        }
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }

  depends_on = [google_project_service.cloud_run_googleapis_com]
}

resource "google_cloud_run_service" "product-api" {
  name     = "product-api"
  location = var.region
  project  = var.project_id

  autogenerate_revision_name = true

  lifecycle {
    ignore_changes = [template[0].spec[0].containers[0].image]
  }

  template {
    spec {
      containers {
        image = "eu.gcr.io/beatutyscrap/product-api:latest"

        ports {
          container_port = var.port
        }

        resources {
          limits = {
            cpu    = "2000m"
            memory = "1Gi"
          }
        }

        env {
          name  = "POSTGRES_CON_STR"
          value = ""
        }

        env {
          name  = "ASPNETCORE_ENVIRONMENT"
          value = "Production"
        }
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }

  depends_on = [google_project_service.cloud_run_googleapis_com]
}

resource "google_cloud_run_service" "subscriber-api" {
  name     = "subscriber-api"
  location = var.region
  project  = var.project_id

  autogenerate_revision_name = true

  lifecycle {
    ignore_changes = [template[0].spec[0].containers[0].image]
  }

  template {
    spec {
      containers {
        image = "eu.gcr.io/beatutyscrap/subscriber-api:latest"

        ports {
          container_port = var.port
        }

        resources {
          limits = {
            cpu    = "2000m"
            memory = "1Gi"
          }
        }

        env {
          name  = "ATLAS_URI"
          value = local.atlas_uri
        }

        env {
          name  = "ASPNETCORE_ENVIRONMENT"
          value = "Production"
        }
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }

  depends_on = [google_project_service.cloud_run_googleapis_com]
}

# Allow unauthenticated users to invoke the service
resource "google_cloud_run_service_iam_member" "run_all_users" {
  service  = google_cloud_run_service.bot.name
  location = google_cloud_run_service.bot.location
  role     = "roles/run.invoker"
  member   = "allUsers"
} 