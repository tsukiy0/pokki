import React from "react";
import { AppProps } from "next/app";
import { Blueprint } from "../components/Blueprint";
import { ConfigContextProvider } from "../contexts/ConfigContext";

const App: React.FC<AppProps> = ({ Component, pageProps }) => {
  return (
    <Blueprint isDark>
      <ConfigContextProvider>
        {/* eslint-disable-next-line react/jsx-props-no-spreading */}
        <Component {...pageProps} />
      </ConfigContextProvider>
    </Blueprint>
  );
};

export default App;
