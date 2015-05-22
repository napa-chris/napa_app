using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlchemyAPI
{
    public class AlchemyAPI_NewsParams : AlchemyAPI_BaseParams
    {
        private const string TAXONOMY_LABEL = "q.enriched.url.enrichedTitle.taxonomy.taxonomy_.label";
        private const string CONCEPT_TEXT = "q.enriched.url.concepts.concept.text";
        private const string KEYWORD_TEXT = "q.enriched.url.enrichedTitle.keywords.keyword.text";
        private const string ENTITY_TEXT = "q.enriched.url.enrichedTitle.entities.entity.text";
        private const string ENTITY_TYPE = "q.enriched.url.enrichedTitle.entities.entity.type";
        private const string RELATION = "q.enriched.url.enrichedTitle.relations.relation";
        private const string SENTIMENT = "q.enriched.url.enrichedTitle.docSentiment";
        private const string COUNT = "count";
        private const string START_DATE = "start";
        private const string END_DATE = "end";

        private readonly Dictionary<string, string> _commands;

        public String StartDate
        {
            set { _commands[START_DATE] = value; }
        }

        public String EndDate
        {
            set { _commands[END_DATE] = value; }
        }

        public int Count
        {
            set { _commands[COUNT] = value > 0 ? value.ToString() : "0"; }
        }

        public AlchemyAPI_NewsParams()
        {
            _commands = new Dictionary<string, string>();
        }

        public void initializeCommands()
        {
            _commands[START_DATE] = "now";
            _commands[END_DATE] = "now";
            _commands[COUNT] = "0";
        }

        public void setTaxonomy(params string[] s)
        {
            _commands[TAXONOMY_LABEL] = String.Join("+", s);
        }

        public void setEntities(params Entity[] entities)
        {
            _commands[ENTITY_TEXT] = String.Join("+", entities.Select(e => e.Text.Replace(' ', '+')));
            _commands[ENTITY_TYPE] = String.Join("+", entities.Select(e => e.Type));
        }

        public override String getParameterString()
        {
            return String.Format("{0}&{1}", base.getParameterString(), String.Join("&", _commands.Select(e => String.Format("{0}={1}", e.Key, e.Value))));
        }
    }
}

public enum EntityType
{
    Person,
    Company
}

public class Entity
{
    public EntityType Type { get; set; }
    public string Text { get; set; }
    public Entity(EntityType type, string text)
    {
        Type = type;
        Text = text;
    }
}

public enum SentimentType
{
    Negative,
    Positive
}

public enum ScoreEquality
{
    GreaterThan,
    LessThan
}