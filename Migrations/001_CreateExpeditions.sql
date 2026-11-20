CREATE TABLE expeditions
(
    id UUID PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP,
    company_rating INT NOT NULL DEFAULT 0
        CHECK (company_rating BETWEEN 0 AND 10)
);

CREATE TABLE adventurers
(
    id UUID PRIMARY KEY,
    expedition_id UUID,
    name TEXT NOT NULL,
    archetype INTEGER NOT NULL DEFAULT 0,
    sex INTEGER NOT NULL DEFAULT 0,
    is_live BOOLEAN NOT NULL DEFAULT TRUE,
    rating INT NOT NULL DEFAULT 0
        CHECK (rating BETWEEN 0 AND 100),
    
    CONSTRAINT fk_adventurers_expeditions
        FOREIGN KEY (expedition_id)
            REFERENCES expeditions(id)
);