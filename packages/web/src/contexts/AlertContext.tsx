import React, { useContext } from "react";
import { Toaster, Position, Intent } from "@blueprintjs/core";

type Value = {
  onSuccess: (message: string) => void;
  onError: (error: Error) => void;
};

export const AlertContext = React.createContext<Value>({} as Value);

const toaster = process.browser
  ? Toaster.create({
      position: Position.TOP,
    })
  : undefined;

export const AlertContextProvider: React.FC = ({ children }) => {
  const onSuccess = (message: string) => {
    toaster?.show({
      intent: Intent.SUCCESS,
      icon: "tick-circle",
      message,
    });
  };

  const onError = (error: Error) => {
    toaster?.show({
      intent: Intent.DANGER,
      icon: "warning-sign",
      message: error.message,
    });
  };

  return (
    <AlertContext.Provider
      value={{
        onSuccess,
        onError,
      }}
    >
      {children}
    </AlertContext.Provider>
  );
};

export const useAlertContext = (): Value => {
  return useContext(AlertContext);
};
