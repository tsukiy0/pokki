import React from "react";
import { AppProps } from "next/app";
import { Blueprint } from "../components/Blueprint";

const App: React.FC<AppProps> = ({ Component, pageProps }) => {
  return (
    <Blueprint isDark>
      {/* eslint-disable-next-line react/jsx-props-no-spreading */}
      <Component {...pageProps} />
    </Blueprint>
  );
};

export default App;
