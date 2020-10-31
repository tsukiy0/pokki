import { GraphQlGameService, GraphQlUserService } from "@pokki/frontend";
import React, { useContext, useEffect, useState } from "react";
import AWSAppSyncClient, { AUTH_TYPE } from "aws-appsync";
import { LoadingPage } from "../components/LoadingPage";
import { useConfigContext } from "./ConfigContext";

type Services = {
  gameService: GraphQlGameService;
  userService: GraphQlUserService;
};

const ServiceContext = React.createContext<Services>({} as any);

export const ServiceContextProvider: React.FC = ({ children }) => {
  const config = useConfigContext();
  const [services, setServices] = useState<Services | undefined>(undefined);

  useEffect(() => {
    const client = new AWSAppSyncClient({
      url: config.get("API_URL"),
      region: config.get("API_REGION"),
      auth: {
        type: AUTH_TYPE.API_KEY,
        apiKey: config.get("API_KEY"),
      },
      disableOffline: true,
    });

    const gameService = new GraphQlGameService(client);
    const userService = new GraphQlUserService(client);

    setServices({
      userService,
      gameService,
    });
  });

  if (!services) {
    return <LoadingPage />;
  }

  return (
    <ServiceContext.Provider value={services}>
      {children}
    </ServiceContext.Provider>
  );
};

export const useServiceContext = () => {
  return useContext(ServiceContext);
};
