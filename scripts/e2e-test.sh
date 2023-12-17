#! /bin/bash

# prep selenium script
npm install --prefix ../src/Node-E2E

# initialize services
docker compose --file ../e2e-compose.yaml up --build -d

# wait for services to be ready
sleep 10

# run test script
npm test --prefix ../src/Node-E2E

# clean up
docker compose --file ../e2e-compose.yaml down