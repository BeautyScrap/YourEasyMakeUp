resource "google_project_service" "cloudscheduler_googleapis_com" {
  service = "cloudscheduler.googleapis.com"
}

resource "google_project_service" "cloud_run_googleapis_com" {
  service = "run.googleapis.com"
}

resource "google_project_service" "artifactregistry_googleapis_com" {
  service = "artifactregistry.googleapis.com"
}

resource "google_project_service" "secretmanager_googleapis_com" {
  service = "secretmanager.googleapis.com"
}

resource "google_project_service" "storage" {
  service = "storage.googleapis.com"
}