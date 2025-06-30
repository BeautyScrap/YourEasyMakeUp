terraform {
  backend "gcs" {
    bucket = "beauty-scrp-bucket-tfstate"
    prefix = "terraform/state"
  }

  required_version = ">= 0.14"

  required_providers {
    google = {
      version = ">= 3.3"
      source  = "hashicorp/google"
    }
  }
}

provider "google" {
  project = var.project_id
  region  = var.region
}