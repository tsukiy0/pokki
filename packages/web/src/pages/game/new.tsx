import { CardSet } from "@pokki/core";
import React, { useState } from "react";
import { CardSetInput } from "../../components/CardSetInput";

const NewGame: React.FC = () => {
  const [cardSet, setCardSet] = useState(new CardSet([]));

  return (
    <div>
      <CardSetInput value={cardSet} onChange={setCardSet} />
    </div>
  );
};

export default NewGame;
