import { Config } from "@tsukiy0/tscore";
import { FrontendConfig } from "@pokki/frontend";
import React, { useEffect, useState } from "react";
import { LoadingPage } from "../components/LoadingPage";

type Value = {
  config: Config;
};

const ConfigContext = React.createContext<Value>({} as any);

export const ConfigContextProvider: React.FC = ({ children }) => {
  const [config, setConfig] = useState<Config | undefined>(undefined);

  useEffect(() => {
    (async () => {
      const c =
        process.env.NODE_ENV === "development"
          ? new FrontendConfig({
              API_KEY: process.env.NEXT_PUBLIC_API_KEY as string,
              API_URL: process.env.NEXT_PUBLIC_API_URL as string,
              API_REGION: process.env.NEXT_PUBLIC_API_REGION as string,
            })
          : await FrontendConfig.fromRemote();

      setConfig(c);
    })();
  }, []);

  if (!config) {
    return <LoadingPage />;
  }

  return (
    <ConfigContext.Provider
      value={{
        config,
      }}
    >
      {children}
    </ConfigContext.Provider>
  );
};
