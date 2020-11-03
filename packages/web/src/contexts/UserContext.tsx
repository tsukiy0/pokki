import { User, UserIdRandomizer, UserSerializer } from "@pokki/core";
import React, { useContext, useEffect, useState } from "react";
import { Card, FormGroup, InputGroup, Button } from "@blueprintjs/core";
import { useServiceContext } from "./ServiceContext";
import { useAlertContext } from "./AlertContext";

const UserContext = React.createContext<User>({} as any);

export const UserContextProvider: React.FC = ({ children }) => {
  const { userService } = useServiceContext();
  const { onError, onSuccess } = useAlertContext();
  const [user, setUser] = useState<User>();
  const [name, setName] = useState("");
  const localStorageKey = "user";

  useEffect(() => {
    const value = localStorage.getItem(localStorageKey);

    if (value) {
      setUser(UserSerializer.deserialize(JSON.parse(value)));
    }
  }, []);

  useEffect(() => {
    if (user) {
      localStorage.setItem(
        localStorageKey,
        JSON.stringify(UserSerializer.serialize(user)),
      );
    }
  }, [user]);

  const onCreate = async () => {
    try {
      const newUser = new User(UserIdRandomizer.random(), name);
      await userService.createUser(newUser);
      setUser(newUser);
      onSuccess("user created");
    } catch (err) {
      onError(err);
    }
  };

  if (!user) {
    return (
      <Card>
        <FormGroup label="Name" labelFor="name">
          <InputGroup
            id="name"
            value={name}
            onChange={(e: any) => setName(e.target.value)}
          />
        </FormGroup>
        <Button onClick={onCreate}>Create</Button>
      </Card>
    );
  }

  return <UserContext.Provider value={user}>{children}</UserContext.Provider>;
};

export const useUserContext = (): User => {
  return useContext(UserContext);
};
