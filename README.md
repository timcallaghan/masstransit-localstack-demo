# MassTransit LocalStack Demo

Demonstration of using [MassTransit](https://masstransit.io/) with [LocalStack](https://github.com/localstack/localstack) emulating AWS SNS and SQS.

## Setup

1. Ensure docker is installed and then run `docker compose up -d` to start up LocalStack.
2. Open the sln in your favourite dotnet IDE (e.g. Rider) and click run/debug
3. Messages will be published to the bus (SNS->SQS) and consumed by the consumer

## Using AWS CLI

To query localstack using the AWS CLI, first create a named profile:

```shell
aws configure --profile localstack
AWS Access Key ID [None]: test
AWS Secret Access Key [None]: test
Default region name [None]: ap-southeast-2
Default output format [None]: json
```

Then you can run commands like the following

```shell
aws --endpoint-url=http://localhost:4566 --profile localstack sqs list-queues
```