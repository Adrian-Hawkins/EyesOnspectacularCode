terraform {
  backend "s3" {
    bucket = "eosc-tfstate"
    key    = "terraform.tfstate"
    region = "eu-west-1"
  }
}