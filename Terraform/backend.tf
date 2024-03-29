terraform {
  backend "s3" {
    bucket = "bucket-name"
    key    = "terraform.tfstate"
    region = "eu-west-1"
  }
}