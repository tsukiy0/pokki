import {
  Button,
  ControlGroup,
  FormGroup,
  InputGroup,
  Tag,
} from "@blueprintjs/core";
import { CardSet, Card, CardIdRandomizer, CardId } from "@pokki/core";
import { css } from "emotion";
import React, { useState } from "react";
import { useAlertContext } from "../contexts/AlertContext";
import { BaseProps } from "./BaseProps";

export const CardSetInput: React.FC<BaseProps<{
  value: CardSet;
  onChange: (value: CardSet) => void;
}>> = ({ className, value, onChange }) => {
  const { onError } = useAlertContext();
  const [name, setName] = useState("");

  const onAdd = () => {
    try {
      onChange(value.addCard(new Card(CardIdRandomizer.random(), name)));
      setName("");
    } catch (e) {
      onError(e);
    }
  };

  const onRemove = (id: CardId) => {
    onChange(value.removeCard(id));
  };

  return (
    <FormGroup
      className={className}
      label="Cards"
      labelFor="cardName"
      inline
      contentClassName={css({
        width: "100%",
      })}
    >
      <form>
        <ControlGroup fill>
          <InputGroup
            id="cardName"
            value={name}
            onChange={(e: any) => setName(e.target.value)}
          />
          <Button
            type="submit"
            onClick={(e: any) => {
              e.preventDefault();
              onAdd();
            }}
            icon="plus"
            aria-label="add card"
          />
        </ControlGroup>
      </form>
      {value.items.length > 0 && (
        <div
          className={css({
            marginTop: "1rem",
          })}
        >
          {value.items.map((item) => {
            return (
              <Tag
                className={css({
                  marginRight: "1rem",
                  marginBottom: "1rem",
                })}
                onRemove={() => onRemove(item.id)}
                large
              >
                {item.name}
              </Tag>
            );
          })}
        </div>
      )}
    </FormGroup>
  );
};
