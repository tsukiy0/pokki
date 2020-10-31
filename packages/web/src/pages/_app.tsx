import React from "react";
import { AppProps } from "next/app";
import { Blueprint } from "../components/Blueprint";
import { ConfigContextProvider } from "../contexts/ConfigContext";
import { ServiceContextProvider } from "../contexts/ServiceContext";
import { AlertContext, AlertContextProvider } from "../contexts/AlertContext";

const App: React.FC<AppProps> = ({ Component, pageProps }) => {
  return (
    <Blueprint isDark>
      <AlertContextProvider>
        <ConfigContextProvider>
          <ServiceContextProvider>
            {/* eslint-disable-next-line react/jsx-props-no-spreading */}
            <Component {...pageProps} />
          </ServiceContextProvider>
        </ConfigContextProvider>
      </AlertContextProvider>
    </Blueprint>
  );
};

export default App;
