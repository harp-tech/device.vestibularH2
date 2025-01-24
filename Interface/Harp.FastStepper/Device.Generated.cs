using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.FastStepper
{
    /// <summary>
    /// Generates events and processes commands for the FastStepper device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the FastStepper device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="FastStepper"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 2120;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(FastStepper);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(Control) },
            { 33, typeof(Pulses) },
            { 34, typeof(NominalPulseInterval) },
            { 35, typeof(InitialPulseInterval) },
            { 36, typeof(PulseStepInterval) },
            { 37, typeof(PulsePeriod) },
            { 38, typeof(Encoder) },
            { 39, typeof(AnalogInput) },
            { 40, typeof(StopSwitch) },
            { 41, typeof(MotorState) },
            { 42, typeof(ImmediatePulses) }
        };
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="FastStepper"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of FastStepper messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="FastStepper"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="FastStepper"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="FastStepper"/> device.
    /// </summary>
    /// <seealso cref="Control"/>
    /// <seealso cref="Pulses"/>
    /// <seealso cref="NominalPulseInterval"/>
    /// <seealso cref="InitialPulseInterval"/>
    /// <seealso cref="PulseStepInterval"/>
    /// <seealso cref="PulsePeriod"/>
    /// <seealso cref="Encoder"/>
    /// <seealso cref="AnalogInput"/>
    /// <seealso cref="StopSwitch"/>
    /// <seealso cref="MotorState"/>
    /// <seealso cref="ImmediatePulses"/>
    [XmlInclude(typeof(Control))]
    [XmlInclude(typeof(Pulses))]
    [XmlInclude(typeof(NominalPulseInterval))]
    [XmlInclude(typeof(InitialPulseInterval))]
    [XmlInclude(typeof(PulseStepInterval))]
    [XmlInclude(typeof(PulsePeriod))]
    [XmlInclude(typeof(Encoder))]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(StopSwitch))]
    [XmlInclude(typeof(MotorState))]
    [XmlInclude(typeof(ImmediatePulses))]
    [Description("Filters register-specific messages reported by the FastStepper device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new Control();
        }

        string INamedElement.Name
        {
            get => $"{nameof(FastStepper)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the FastStepper device.
    /// </summary>
    /// <seealso cref="Control"/>
    /// <seealso cref="Pulses"/>
    /// <seealso cref="NominalPulseInterval"/>
    /// <seealso cref="InitialPulseInterval"/>
    /// <seealso cref="PulseStepInterval"/>
    /// <seealso cref="PulsePeriod"/>
    /// <seealso cref="Encoder"/>
    /// <seealso cref="AnalogInput"/>
    /// <seealso cref="StopSwitch"/>
    /// <seealso cref="MotorState"/>
    /// <seealso cref="ImmediatePulses"/>
    [XmlInclude(typeof(Control))]
    [XmlInclude(typeof(Pulses))]
    [XmlInclude(typeof(NominalPulseInterval))]
    [XmlInclude(typeof(InitialPulseInterval))]
    [XmlInclude(typeof(PulseStepInterval))]
    [XmlInclude(typeof(PulsePeriod))]
    [XmlInclude(typeof(Encoder))]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(StopSwitch))]
    [XmlInclude(typeof(MotorState))]
    [XmlInclude(typeof(ImmediatePulses))]
    [XmlInclude(typeof(TimestampedControl))]
    [XmlInclude(typeof(TimestampedPulses))]
    [XmlInclude(typeof(TimestampedNominalPulseInterval))]
    [XmlInclude(typeof(TimestampedInitialPulseInterval))]
    [XmlInclude(typeof(TimestampedPulseStepInterval))]
    [XmlInclude(typeof(TimestampedPulsePeriod))]
    [XmlInclude(typeof(TimestampedEncoder))]
    [XmlInclude(typeof(TimestampedAnalogInput))]
    [XmlInclude(typeof(TimestampedStopSwitch))]
    [XmlInclude(typeof(TimestampedMotorState))]
    [XmlInclude(typeof(TimestampedImmediatePulses))]
    [Description("Filters and selects specific messages reported by the FastStepper device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new Control();
        }

        string INamedElement.Name => $"{nameof(FastStepper)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// FastStepper register messages.
    /// </summary>
    /// <seealso cref="Control"/>
    /// <seealso cref="Pulses"/>
    /// <seealso cref="NominalPulseInterval"/>
    /// <seealso cref="InitialPulseInterval"/>
    /// <seealso cref="PulseStepInterval"/>
    /// <seealso cref="PulsePeriod"/>
    /// <seealso cref="Encoder"/>
    /// <seealso cref="AnalogInput"/>
    /// <seealso cref="StopSwitch"/>
    /// <seealso cref="MotorState"/>
    /// <seealso cref="ImmediatePulses"/>
    [XmlInclude(typeof(Control))]
    [XmlInclude(typeof(Pulses))]
    [XmlInclude(typeof(NominalPulseInterval))]
    [XmlInclude(typeof(InitialPulseInterval))]
    [XmlInclude(typeof(PulseStepInterval))]
    [XmlInclude(typeof(PulsePeriod))]
    [XmlInclude(typeof(Encoder))]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(StopSwitch))]
    [XmlInclude(typeof(MotorState))]
    [XmlInclude(typeof(ImmediatePulses))]
    [Description("Formats a sequence of values as specific FastStepper register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new Control();
        }

        string INamedElement.Name => $"{nameof(FastStepper)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that control the device modules.
    /// </summary>
    [Description("Control the device modules.")]
    public partial class Control
    {
        /// <summary>
        /// Represents the address of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Control"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ControlFlags GetPayload(HarpMessage message)
        {
            return (ControlFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Control"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ControlFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ControlFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Control"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Control"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ControlFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Control"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Control"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ControlFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Control register.
    /// </summary>
    /// <seealso cref="Control"/>
    [Description("Filters and selects timestamped messages from the Control register.")]
    public partial class TimestampedControl
    {
        /// <summary>
        /// Represents the address of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const int Address = Control.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Control"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ControlFlags> GetPayload(HarpMessage message)
        {
            return Control.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sends the number of pulses written in this register and set the direction according to the number's sign.
    /// </summary>
    [Description("Sends the number of pulses written in this register and set the direction according to the number's sign.")]
    public partial class Pulses
    {
        /// <summary>
        /// Represents the address of the <see cref="Pulses"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="Pulses"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="Pulses"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pulses"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pulses"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pulses"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pulses"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pulses"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pulses"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pulses register.
    /// </summary>
    /// <seealso cref="Pulses"/>
    [Description("Filters and selects timestamped messages from the Pulses register.")]
    public partial class TimestampedPulses
    {
        /// <summary>
        /// Represents the address of the <see cref="Pulses"/> register. This field is constant.
        /// </summary>
        public const int Address = Pulses.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pulses"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return Pulses.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the motor pulse interval when running at nominal speed.
    /// </summary>
    [Description("Sets the motor pulse interval when running at nominal speed.")]
    public partial class NominalPulseInterval
    {
        /// <summary>
        /// Represents the address of the <see cref="NominalPulseInterval"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="NominalPulseInterval"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="NominalPulseInterval"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="NominalPulseInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="NominalPulseInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="NominalPulseInterval"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="NominalPulseInterval"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="NominalPulseInterval"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="NominalPulseInterval"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// NominalPulseInterval register.
    /// </summary>
    /// <seealso cref="NominalPulseInterval"/>
    [Description("Filters and selects timestamped messages from the NominalPulseInterval register.")]
    public partial class TimestampedNominalPulseInterval
    {
        /// <summary>
        /// Represents the address of the <see cref="NominalPulseInterval"/> register. This field is constant.
        /// </summary>
        public const int Address = NominalPulseInterval.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="NominalPulseInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return NominalPulseInterval.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.
    /// </summary>
    [Description("Sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.")]
    public partial class InitialPulseInterval
    {
        /// <summary>
        /// Represents the address of the <see cref="InitialPulseInterval"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="InitialPulseInterval"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="InitialPulseInterval"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="InitialPulseInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="InitialPulseInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="InitialPulseInterval"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="InitialPulseInterval"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="InitialPulseInterval"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="InitialPulseInterval"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// InitialPulseInterval register.
    /// </summary>
    /// <seealso cref="InitialPulseInterval"/>
    [Description("Filters and selects timestamped messages from the InitialPulseInterval register.")]
    public partial class TimestampedInitialPulseInterval
    {
        /// <summary>
        /// Represents the address of the <see cref="InitialPulseInterval"/> register. This field is constant.
        /// </summary>
        public const int Address = InitialPulseInterval.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="InitialPulseInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return InitialPulseInterval.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.
    /// </summary>
    [Description("Sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.")]
    public partial class PulseStepInterval
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseStepInterval"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseStepInterval"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulseStepInterval"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseStepInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseStepInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseStepInterval"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseStepInterval"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseStepInterval"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseStepInterval"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseStepInterval register.
    /// </summary>
    /// <seealso cref="PulseStepInterval"/>
    [Description("Filters and selects timestamped messages from the PulseStepInterval register.")]
    public partial class TimestampedPulseStepInterval
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseStepInterval"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseStepInterval.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseStepInterval"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulseStepInterval.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the period of the pulse.
    /// </summary>
    [Description("Sets the period of the pulse.")]
    public partial class PulsePeriod
    {
        /// <summary>
        /// Represents the address of the <see cref="PulsePeriod"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="PulsePeriod"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PulsePeriod"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulsePeriod"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulsePeriod"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulsePeriod"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulsePeriod"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulsePeriod"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulsePeriod"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulsePeriod register.
    /// </summary>
    /// <seealso cref="PulsePeriod"/>
    [Description("Filters and selects timestamped messages from the PulsePeriod register.")]
    public partial class TimestampedPulsePeriod
    {
        /// <summary>
        /// Represents the address of the <see cref="PulsePeriod"/> register. This field is constant.
        /// </summary>
        public const int Address = PulsePeriod.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulsePeriod"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PulsePeriod.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the reading of the quadrature encoder.
    /// </summary>
    [Description("Contains the reading of the quadrature encoder.")]
    public partial class Encoder
    {
        /// <summary>
        /// Represents the address of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S16;

        /// <summary>
        /// Represents the length of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Encoder"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static short GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Encoder"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Encoder"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Encoder"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Encoder"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Encoder"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Encoder register.
    /// </summary>
    /// <seealso cref="Encoder"/>
    [Description("Filters and selects timestamped messages from the Encoder register.")]
    public partial class TimestampedEncoder
    {
        /// <summary>
        /// Represents the address of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const int Address = Encoder.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Encoder"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetPayload(HarpMessage message)
        {
            return Encoder.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the reading of the analog input.
    /// </summary>
    [Description("Contains the reading of the analog input.")]
    public partial class AnalogInput
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S16;

        /// <summary>
        /// Represents the length of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AnalogInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static short GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AnalogInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AnalogInput"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogInput"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AnalogInput"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogInput"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AnalogInput register.
    /// </summary>
    /// <seealso cref="AnalogInput"/>
    [Description("Filters and selects timestamped messages from the AnalogInput register.")]
    public partial class TimestampedAnalogInput
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const int Address = AnalogInput.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AnalogInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetPayload(HarpMessage message)
        {
            return AnalogInput.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the state of the stop switch.
    /// </summary>
    [Description("Contains the state of the stop switch.")]
    public partial class StopSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StopSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static StopSwitchFlags GetPayload(HarpMessage message)
        {
            return (StopSwitchFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StopSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StopSwitchFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((StopSwitchFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StopSwitch"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StopSwitch"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, StopSwitchFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StopSwitch"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StopSwitch"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, StopSwitchFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StopSwitch register.
    /// </summary>
    /// <seealso cref="StopSwitch"/>
    [Description("Filters and selects timestamped messages from the StopSwitch register.")]
    public partial class TimestampedStopSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = StopSwitch.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StopSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StopSwitchFlags> GetPayload(HarpMessage message)
        {
            return StopSwitch.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the state of the motor.
    /// </summary>
    [Description("Contains the state of the motor.")]
    public partial class MotorState
    {
        /// <summary>
        /// Represents the address of the <see cref="MotorState"/> register. This field is constant.
        /// </summary>
        public const int Address = 41;

        /// <summary>
        /// Represents the payload type of the <see cref="MotorState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MotorState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MotorState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MotorStateFlags GetPayload(HarpMessage message)
        {
            return (MotorStateFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MotorState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MotorStateFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MotorStateFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MotorState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MotorState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MotorStateFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MotorState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MotorState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MotorStateFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MotorState register.
    /// </summary>
    /// <seealso cref="MotorState"/>
    [Description("Filters and selects timestamped messages from the MotorState register.")]
    public partial class TimestampedMotorState
    {
        /// <summary>
        /// Represents the address of the <see cref="MotorState"/> register. This field is constant.
        /// </summary>
        public const int Address = MotorState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MotorState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MotorStateFlags> GetPayload(HarpMessage message)
        {
            return MotorState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets immediately the motor pulse interval. The value's sign defines the direction.
    /// </summary>
    [Description("Sets immediately the motor pulse interval. The value's sign defines the direction.")]
    public partial class ImmediatePulses
    {
        /// <summary>
        /// Represents the address of the <see cref="ImmediatePulses"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="ImmediatePulses"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S16;

        /// <summary>
        /// Represents the length of the <see cref="ImmediatePulses"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ImmediatePulses"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static short GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ImmediatePulses"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ImmediatePulses"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ImmediatePulses"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ImmediatePulses"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ImmediatePulses"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ImmediatePulses register.
    /// </summary>
    /// <seealso cref="ImmediatePulses"/>
    [Description("Filters and selects timestamped messages from the ImmediatePulses register.")]
    public partial class TimestampedImmediatePulses
    {
        /// <summary>
        /// Represents the address of the <see cref="ImmediatePulses"/> register. This field is constant.
        /// </summary>
        public const int Address = ImmediatePulses.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ImmediatePulses"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetPayload(HarpMessage message)
        {
            return ImmediatePulses.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// FastStepper device.
    /// </summary>
    /// <seealso cref="CreateControlPayload"/>
    /// <seealso cref="CreatePulsesPayload"/>
    /// <seealso cref="CreateNominalPulseIntervalPayload"/>
    /// <seealso cref="CreateInitialPulseIntervalPayload"/>
    /// <seealso cref="CreatePulseStepIntervalPayload"/>
    /// <seealso cref="CreatePulsePeriodPayload"/>
    /// <seealso cref="CreateEncoderPayload"/>
    /// <seealso cref="CreateAnalogInputPayload"/>
    /// <seealso cref="CreateStopSwitchPayload"/>
    /// <seealso cref="CreateMotorStatePayload"/>
    /// <seealso cref="CreateImmediatePulsesPayload"/>
    [XmlInclude(typeof(CreateControlPayload))]
    [XmlInclude(typeof(CreatePulsesPayload))]
    [XmlInclude(typeof(CreateNominalPulseIntervalPayload))]
    [XmlInclude(typeof(CreateInitialPulseIntervalPayload))]
    [XmlInclude(typeof(CreatePulseStepIntervalPayload))]
    [XmlInclude(typeof(CreatePulsePeriodPayload))]
    [XmlInclude(typeof(CreateEncoderPayload))]
    [XmlInclude(typeof(CreateAnalogInputPayload))]
    [XmlInclude(typeof(CreateStopSwitchPayload))]
    [XmlInclude(typeof(CreateMotorStatePayload))]
    [XmlInclude(typeof(CreateImmediatePulsesPayload))]
    [XmlInclude(typeof(CreateTimestampedControlPayload))]
    [XmlInclude(typeof(CreateTimestampedPulsesPayload))]
    [XmlInclude(typeof(CreateTimestampedNominalPulseIntervalPayload))]
    [XmlInclude(typeof(CreateTimestampedInitialPulseIntervalPayload))]
    [XmlInclude(typeof(CreateTimestampedPulseStepIntervalPayload))]
    [XmlInclude(typeof(CreateTimestampedPulsePeriodPayload))]
    [XmlInclude(typeof(CreateTimestampedEncoderPayload))]
    [XmlInclude(typeof(CreateTimestampedAnalogInputPayload))]
    [XmlInclude(typeof(CreateTimestampedStopSwitchPayload))]
    [XmlInclude(typeof(CreateTimestampedMotorStatePayload))]
    [XmlInclude(typeof(CreateTimestampedImmediatePulsesPayload))]
    [Description("Creates standard message payloads for the FastStepper device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateControlPayload();
        }

        string INamedElement.Name => $"{nameof(FastStepper)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that control the device modules.
    /// </summary>
    [DisplayName("ControlPayload")]
    [Description("Creates a message payload that control the device modules.")]
    public partial class CreateControlPayload
    {
        /// <summary>
        /// Gets or sets the value that control the device modules.
        /// </summary>
        [Description("The value that control the device modules.")]
        public ControlFlags Control { get; set; }

        /// <summary>
        /// Creates a message payload for the Control register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ControlFlags GetPayload()
        {
            return Control;
        }

        /// <summary>
        /// Creates a message that control the device modules.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Control register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Control.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that control the device modules.
    /// </summary>
    [DisplayName("TimestampedControlPayload")]
    [Description("Creates a timestamped message payload that control the device modules.")]
    public partial class CreateTimestampedControlPayload : CreateControlPayload
    {
        /// <summary>
        /// Creates a timestamped message that control the device modules.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Control register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Control.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sends the number of pulses written in this register and set the direction according to the number's sign.
    /// </summary>
    [DisplayName("PulsesPayload")]
    [Description("Creates a message payload that sends the number of pulses written in this register and set the direction according to the number's sign.")]
    public partial class CreatePulsesPayload
    {
        /// <summary>
        /// Gets or sets the value that sends the number of pulses written in this register and set the direction according to the number's sign.
        /// </summary>
        [Description("The value that sends the number of pulses written in this register and set the direction according to the number's sign.")]
        public int Pulses { get; set; }

        /// <summary>
        /// Creates a message payload for the Pulses register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return Pulses;
        }

        /// <summary>
        /// Creates a message that sends the number of pulses written in this register and set the direction according to the number's sign.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pulses register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Pulses.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sends the number of pulses written in this register and set the direction according to the number's sign.
    /// </summary>
    [DisplayName("TimestampedPulsesPayload")]
    [Description("Creates a timestamped message payload that sends the number of pulses written in this register and set the direction according to the number's sign.")]
    public partial class CreateTimestampedPulsesPayload : CreatePulsesPayload
    {
        /// <summary>
        /// Creates a timestamped message that sends the number of pulses written in this register and set the direction according to the number's sign.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pulses register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Pulses.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the motor pulse interval when running at nominal speed.
    /// </summary>
    [DisplayName("NominalPulseIntervalPayload")]
    [Description("Creates a message payload that sets the motor pulse interval when running at nominal speed.")]
    public partial class CreateNominalPulseIntervalPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the motor pulse interval when running at nominal speed.
        /// </summary>
        [Description("The value that sets the motor pulse interval when running at nominal speed.")]
        public ushort NominalPulseInterval { get; set; }

        /// <summary>
        /// Creates a message payload for the NominalPulseInterval register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return NominalPulseInterval;
        }

        /// <summary>
        /// Creates a message that sets the motor pulse interval when running at nominal speed.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the NominalPulseInterval register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.NominalPulseInterval.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the motor pulse interval when running at nominal speed.
    /// </summary>
    [DisplayName("TimestampedNominalPulseIntervalPayload")]
    [Description("Creates a timestamped message payload that sets the motor pulse interval when running at nominal speed.")]
    public partial class CreateTimestampedNominalPulseIntervalPayload : CreateNominalPulseIntervalPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the motor pulse interval when running at nominal speed.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the NominalPulseInterval register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.NominalPulseInterval.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.
    /// </summary>
    [DisplayName("InitialPulseIntervalPayload")]
    [Description("Creates a message payload that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.")]
    public partial class CreateInitialPulseIntervalPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.
        /// </summary>
        [Description("The value that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.")]
        public ushort InitialPulseInterval { get; set; }

        /// <summary>
        /// Creates a message payload for the InitialPulseInterval register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return InitialPulseInterval;
        }

        /// <summary>
        /// Creates a message that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the InitialPulseInterval register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.InitialPulseInterval.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.
    /// </summary>
    [DisplayName("TimestampedInitialPulseIntervalPayload")]
    [Description("Creates a timestamped message payload that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.")]
    public partial class CreateTimestampedInitialPulseIntervalPayload : CreateInitialPulseIntervalPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the motor's maximum pulse interval, used as the first and last pulse interval of a rotation.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the InitialPulseInterval register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.InitialPulseInterval.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.
    /// </summary>
    [DisplayName("PulseStepIntervalPayload")]
    [Description("Creates a message payload that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.")]
    public partial class CreatePulseStepIntervalPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.
        /// </summary>
        [Description("The value that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.")]
        public ushort PulseStepInterval { get; set; }

        /// <summary>
        /// Creates a message payload for the PulseStepInterval register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulseStepInterval;
        }

        /// <summary>
        /// Creates a message that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseStepInterval register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.PulseStepInterval.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.
    /// </summary>
    [DisplayName("TimestampedPulseStepIntervalPayload")]
    [Description("Creates a timestamped message payload that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.")]
    public partial class CreateTimestampedPulseStepIntervalPayload : CreatePulseStepIntervalPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the acceleration. The pulse's interval is decreased by this value when accelerating and increased when de-accelerating.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseStepInterval register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.PulseStepInterval.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the period of the pulse.
    /// </summary>
    [DisplayName("PulsePeriodPayload")]
    [Description("Creates a message payload that sets the period of the pulse.")]
    public partial class CreatePulsePeriodPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the period of the pulse.
        /// </summary>
        [Description("The value that sets the period of the pulse.")]
        public ushort PulsePeriod { get; set; }

        /// <summary>
        /// Creates a message payload for the PulsePeriod register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PulsePeriod;
        }

        /// <summary>
        /// Creates a message that sets the period of the pulse.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulsePeriod register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.PulsePeriod.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the period of the pulse.
    /// </summary>
    [DisplayName("TimestampedPulsePeriodPayload")]
    [Description("Creates a timestamped message payload that sets the period of the pulse.")]
    public partial class CreateTimestampedPulsePeriodPayload : CreatePulsePeriodPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the period of the pulse.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulsePeriod register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.PulsePeriod.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the reading of the quadrature encoder.
    /// </summary>
    [DisplayName("EncoderPayload")]
    [Description("Creates a message payload that contains the reading of the quadrature encoder.")]
    public partial class CreateEncoderPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the reading of the quadrature encoder.
        /// </summary>
        [Description("The value that contains the reading of the quadrature encoder.")]
        public short Encoder { get; set; }

        /// <summary>
        /// Creates a message payload for the Encoder register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public short GetPayload()
        {
            return Encoder;
        }

        /// <summary>
        /// Creates a message that contains the reading of the quadrature encoder.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Encoder register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Encoder.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the reading of the quadrature encoder.
    /// </summary>
    [DisplayName("TimestampedEncoderPayload")]
    [Description("Creates a timestamped message payload that contains the reading of the quadrature encoder.")]
    public partial class CreateTimestampedEncoderPayload : CreateEncoderPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the reading of the quadrature encoder.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Encoder register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Encoder.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the reading of the analog input.
    /// </summary>
    [DisplayName("AnalogInputPayload")]
    [Description("Creates a message payload that contains the reading of the analog input.")]
    public partial class CreateAnalogInputPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the reading of the analog input.
        /// </summary>
        [Description("The value that contains the reading of the analog input.")]
        public short AnalogInput { get; set; }

        /// <summary>
        /// Creates a message payload for the AnalogInput register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public short GetPayload()
        {
            return AnalogInput;
        }

        /// <summary>
        /// Creates a message that contains the reading of the analog input.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AnalogInput register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.AnalogInput.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the reading of the analog input.
    /// </summary>
    [DisplayName("TimestampedAnalogInputPayload")]
    [Description("Creates a timestamped message payload that contains the reading of the analog input.")]
    public partial class CreateTimestampedAnalogInputPayload : CreateAnalogInputPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the reading of the analog input.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AnalogInput register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.AnalogInput.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the state of the stop switch.
    /// </summary>
    [DisplayName("StopSwitchPayload")]
    [Description("Creates a message payload that contains the state of the stop switch.")]
    public partial class CreateStopSwitchPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the state of the stop switch.
        /// </summary>
        [Description("The value that contains the state of the stop switch.")]
        public StopSwitchFlags StopSwitch { get; set; }

        /// <summary>
        /// Creates a message payload for the StopSwitch register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public StopSwitchFlags GetPayload()
        {
            return StopSwitch;
        }

        /// <summary>
        /// Creates a message that contains the state of the stop switch.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StopSwitch register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.StopSwitch.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the state of the stop switch.
    /// </summary>
    [DisplayName("TimestampedStopSwitchPayload")]
    [Description("Creates a timestamped message payload that contains the state of the stop switch.")]
    public partial class CreateTimestampedStopSwitchPayload : CreateStopSwitchPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the state of the stop switch.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StopSwitch register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.StopSwitch.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the state of the motor.
    /// </summary>
    [DisplayName("MotorStatePayload")]
    [Description("Creates a message payload that contains the state of the motor.")]
    public partial class CreateMotorStatePayload
    {
        /// <summary>
        /// Gets or sets the value that contains the state of the motor.
        /// </summary>
        [Description("The value that contains the state of the motor.")]
        public MotorStateFlags MotorState { get; set; }

        /// <summary>
        /// Creates a message payload for the MotorState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MotorStateFlags GetPayload()
        {
            return MotorState;
        }

        /// <summary>
        /// Creates a message that contains the state of the motor.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MotorState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.MotorState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the state of the motor.
    /// </summary>
    [DisplayName("TimestampedMotorStatePayload")]
    [Description("Creates a timestamped message payload that contains the state of the motor.")]
    public partial class CreateTimestampedMotorStatePayload : CreateMotorStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the state of the motor.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MotorState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.MotorState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets immediately the motor pulse interval. The value's sign defines the direction.
    /// </summary>
    [DisplayName("ImmediatePulsesPayload")]
    [Description("Creates a message payload that sets immediately the motor pulse interval. The value's sign defines the direction.")]
    public partial class CreateImmediatePulsesPayload
    {
        /// <summary>
        /// Gets or sets the value that sets immediately the motor pulse interval. The value's sign defines the direction.
        /// </summary>
        [Description("The value that sets immediately the motor pulse interval. The value's sign defines the direction.")]
        public short ImmediatePulses { get; set; }

        /// <summary>
        /// Creates a message payload for the ImmediatePulses register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public short GetPayload()
        {
            return ImmediatePulses;
        }

        /// <summary>
        /// Creates a message that sets immediately the motor pulse interval. The value's sign defines the direction.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ImmediatePulses register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.ImmediatePulses.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets immediately the motor pulse interval. The value's sign defines the direction.
    /// </summary>
    [DisplayName("TimestampedImmediatePulsesPayload")]
    [Description("Creates a timestamped message payload that sets immediately the motor pulse interval. The value's sign defines the direction.")]
    public partial class CreateTimestampedImmediatePulsesPayload : CreateImmediatePulsesPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets immediately the motor pulse interval. The value's sign defines the direction.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ImmediatePulses register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.ImmediatePulses.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Available device module configuration flags.
    /// </summary>
    [Flags]
    public enum ControlFlags : byte
    {
        None = 0x0,
        EnableMotor = 0x1,
        DisableMotor = 0x2,
        EnableAnalogInput = 0x4,
        DisableAnalogInput = 0x8,
        EnableEncoder = 0x10,
        DisableEncoder = 0x20,
        ResetEncoder = 0x40
    }

    /// <summary>
    /// Flags describing the state of the motor stop switch.
    /// </summary>
    [Flags]
    public enum StopSwitchFlags : byte
    {
        None = 0x0,
        StopSwitch = 0x1
    }

    /// <summary>
    /// Flags describing the movement state of the motor.
    /// </summary>
    [Flags]
    public enum MotorStateFlags : byte
    {
        None = 0x0,
        IsMoving = 0x1
    }
}
