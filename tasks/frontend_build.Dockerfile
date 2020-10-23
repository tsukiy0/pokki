FROM node:12

RUN npm install -g @aws-amplify/cli@4.30
RUN npm install -g amplify-codegen@2.15.21
