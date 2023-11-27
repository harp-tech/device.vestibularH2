using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.VestibularH2
{
    /// <summary>
    /// Generates events and processes commands for the VestibularH2 device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the VestibularH2 device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="VestibularH2"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(VestibularH2);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(DigitalInputs) }
        };
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="VestibularH2"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of VestibularH2 messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="VestibularH2"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="VestibularH2"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="VestibularH2"/> device.
    /// </summary>
    /// <seealso cref="DigitalInputs"/>
    [XmlInclude(typeof(DigitalInputs))]
    [Description("Filters register-specific messages reported by the VestibularH2 device.")]
    public class FilterMessage : FilterMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterMessage"/> class.
        /// </summary>
        public FilterMessage()
        {
            Register = new DigitalInputs();
        }

        string INamedElement.Name
        {
            get => $"{nameof(VestibularH2)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the VestibularH2 device.
    /// </summary>
    /// <seealso cref="DigitalInputs"/>
    [XmlInclude(typeof(DigitalInputs))]
    [XmlInclude(typeof(TimestampedDigitalInputs))]
    [Description("Filters and selects specific messages reported by the VestibularH2 device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new DigitalInputs();
        }

        string INamedElement.Name => $"{nameof(VestibularH2)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// VestibularH2 register messages.
    /// </summary>
    /// <seealso cref="DigitalInputs"/>
    [XmlInclude(typeof(DigitalInputs))]
    [Description("Formats a sequence of values as specific VestibularH2 register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new DigitalInputs();
        }

        string INamedElement.Name => $"{nameof(VestibularH2)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that manipulates messages from register DigitalInputs.
    /// </summary>
    [Description("")]
    public partial class DigitalInputs
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalInputs"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputs"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalInputs"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputs"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalInputs register.
    /// </summary>
    /// <seealso cref="DigitalInputs"/>
    [Description("Filters and selects timestamped messages from the DigitalInputs register.")]
    public partial class TimestampedDigitalInputs
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalInputs.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return DigitalInputs.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// VestibularH2 device.
    /// </summary>
    /// <seealso cref="CreateDigitalInputsPayload"/>
    [XmlInclude(typeof(CreateDigitalInputsPayload))]
    [Description("Creates standard message payloads for the VestibularH2 device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateDigitalInputsPayload();
        }

        string INamedElement.Name => $"{nameof(VestibularH2)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// for register DigitalInputs.
    /// </summary>
    [DisplayName("DigitalInputsPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads for register DigitalInputs.")]
    public partial class CreateDigitalInputsPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value for register DigitalInputs.
        /// </summary>
        [Description("The value for register DigitalInputs.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// for register DigitalInputs.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// for register DigitalInputs.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => DigitalInputs.FromPayload(MessageType, Value));
        }
    }
}
