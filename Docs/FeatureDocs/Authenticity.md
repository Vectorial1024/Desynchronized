So far, we have always assumed that the news are always real. But this is not realistic. What if pawns forget some details, and mistakenly gave the wrong information? What if, defined by some traits, some pawns always spread lies, or selectively lies about their enemies?

The existing system fails to address this problem; it already assumes that all transmitted news are real in themselves. (Refer to [].)

# Authenticity

It is clear that, to allow the existence of the lies themselves, a "true" value and at least one fake value have to be stored, such that pawns may choose between the multiple values available. However, we still need to highlight the distinction of the "true" value from the rest of the fake values, to allow the user to see the effect of lies. This has prompted the following principle:

- **Principle 1**: All information about the authenticity of a piece of news (e.g., the lies and "alternative facts") should be stored in news-references.

The thinking is trivial: both fake values and news-references are pawns' individual interpretations of the associated news, so they should be grouped together.

We have just confirmed the existence (and the need of the existence) of fake values, which means there is a need to determine whether a given news instance is authentic or not, whether the given news instance is free of any "fake" values.

Hence, I define:

- **Definition 1**: **Authenticity Score** is the measure of how authentic a given news instance is. Higher Authenticity Score corresponds to stronger authenticity.

We have just established a criteria to determine the truthfulness of a certain news. But still, this is not realistic: in real life, it is possible that the same news be interpreted in different ways. This requires a need to represent how strong a certain view/theory is, regarding the news itself.

Hence, I define:

- **Definition 2**: A **Probability Object** is an object used to store a list of "possible values", with a corresponding Authenticity Score assigned to each of the "possible values".

Currently, the Probability Objects are to be used to describe, e.g., the possible instigators of a certain murder.

To better integrate Authenticity Score into Probability Objects, I further impose:

- **Definition 3**: Authenticity Score is always a decimal inclusively bounded by both 0 and 1; a score of 1 indicates absolute authenticity, while a score of 0 indicates absolute lies.

In real life, people may have distinct, yet simultaneous, theories of a given news instance. Some of those theories may be more believable, while some others are less so. When combined together, they represent the entirety of people's understanding. In other words, the combination of all those discrete theories form the true authenticity of the news instance to the individual people.

To represent this in our mod, this lemma is resulted:

- **Lemma 1**: The Authenticity Score of the individual entries of a Probability Object used to represent any informational variable of any news instance/news-reference (such as who killed who) always add up to 1.

For example, suppose, in a colony of 4, with colonists A, B, C, D, one of the colonists was killed by another colonist; let's say colonist A died as a result. Now, it would be OK to say either B, C, or D killed A. Then, to represent the killer of this event, we would have:

- Colonist B; AS = b
- Colonist C; AS = c
- Colonist D; AS = d
- 0 <= b, c, d <= 1 (Definition 3)

By Lemma 1, we can immediately establish that b + c + d = 1.

#  Authenticity and News Spread

Currently, news may be spread when pawns conduct some social interactions. I believe that, when news is spread, the AS of the news on the listener side should be affected by the speaker.

But before that, we must first define some more things about the Authenticity Score:

- **Definition 4**: When a pawn witnesses the news first-hand, the news will have an Authenticity Score of 1. (May be changed to consider more factors)

This ensures that the math related to Authenticity Score is always calculable.

Currently, there are several cases to handle for calculating the Authenticity Score for the listener's side when a news is spread. But, a general rule holds:

- **Lemma 2**: When news is spread, the Authenticity Score at the listener side depends on the Authenticiy Score at the speaker side.

- Case 1: Listener has absolutely no knowledge of the news. Result = (AS of speaker) / 2
  - This corresponds to us half-believing things when we have no way of verifying it.
- Case 2: Listener has some knowledge, and speaker has higher AS. Result = average of both AS
  - This corresponds to the listener getting educated on the news by the speaker.
- Case 3: Listener has some knowledge, but speaker has lower AS. Result = listeners' AS - 0.01
  - This corresponds to the listener slightly doubting his belief; the effect should not be significant if the listener has high AS on his own.

That covers the normal AS calculations.

As time goes by, weak news grows even weaker.

- **Lemma 3**: Depending on the value of the Authenticity Score, the Authenticity Score may change over time.
- **Lemma 4**: The change of Authenticity Score due to time is done every 1 in-game day.

- Case 4: A news with AS less than 0.8 will lose 0.01 AS.
- Case 5: A news with AS less than 0.6 will lose 0.01 AS.
- Case 6: A news with AS less than 0.4 will lose 0.01 AS.

The above three cases are additive.

When Authenticity Score drops too low, it would be quite impossible to continue to hold onto the "original" value. Alternative values would take hold.