import React from "react";
import { AppProps } from "next/app";
import { Blueprint } from "../components/Blueprint";
import { ConfigContextProvider } from "../contexts/ConfigContext";
import { ServiceContextProvider } from "../contexts/ServiceContext";
import { AlertContextProvider } from "../contexts/AlertContext";
import { UserContextProvider } from "../contexts/UserContext";

const App: React.FC<AppProps> = ({ Component, pageProps }) => {
  return (
    <Blueprint isDark>
      <AlertContextProvider>
        <ConfigContextProvider>
          <ServiceContextProvider>
            <UserContextProvider>
              {/* eslint-disable-next-line react/jsx-props-no-spreading */}
              <Component {...pageProps} />
            </UserContextProvider>
          </ServiceContextProvider>
        </ConfigContextProvider>
      </AlertContextProvider>
    </Blueprint>
  );
};

export default App;
